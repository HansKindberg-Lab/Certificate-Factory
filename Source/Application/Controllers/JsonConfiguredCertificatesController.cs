using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Archiving;
using Application.Models.Cryptography.Extensions;
using Application.Models.Views.JsonConfiguredCertificates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class JsonConfiguredCertificatesController : ArchiveKindController
	{
		#region Constructors

		public JsonConfiguredCertificatesController(IFacade facade) : base(facade) { }

		#endregion

		#region Methods

		public virtual async Task<IActionResult> Index()
		{
			var model = new JsonConfiguredCertificatesViewModel();

			this.PopulateArchiveKindDictionary(model.Form);

			return await Task.FromResult(this.View(model));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[SuppressMessage("Maintainability", "CA1506:Avoid excessive class coupling")]
		[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
		public virtual async Task<IActionResult> Index(JsonConfiguredCertificatesForm form)
		{
			ArgumentNullException.ThrowIfNull(form);

			this.PopulateArchiveKindDictionary(form);

			CertificateConstructionTreeOptions constructionTree = null;

			if(form.File != null)
			{
				try
				{
					await using(var stream = form.File.OpenReadStream())
					{
						try
						{
							JsonSerializer.Deserialize<JsonObject>(stream);
						}
						catch(Exception exception)
						{
							throw new InvalidOperationException($"Invalid json. {exception.Message}");
						}
					}

					constructionTree = new CertificateConstructionTreeOptions();

					await using(var stream = form.File.OpenReadStream())
					{
						new ConfigurationBuilder().AddJsonStream(stream).Build().Bind(constructionTree);
					}

					this.ValidateConstructionTree(constructionTree);
				}
				catch(Exception exception)
				{
					this.ModelState.AddModelError(nameof(form.File), $"\"{nameof(form.File)}\" is invalid. {exception.Message}");
					constructionTree = null;
				}
			}

			if(this.ModelState.IsValid)
			{
				try
				{
					var certificates = this.Facade.CertificateFactory.Create(this.Facade.AsymmetricAlgorithmRepository, this.Facade.CertificateConstructionHelper, constructionTree);

					// ReSharper disable PossibleInvalidOperationException
					var archive = this.Facade.ArchiveFactory.Create(certificates, new ArchiveOptions { Flat = form.FlatArchive, Kind = form.ArchiveKind.Value }, form.Password);
					// ReSharper restore PossibleInvalidOperationException

					var file = this.File(archive.Bytes.ToArray(), archive.ContentType, this.Facade.FileNameResolver.Resolve($"{constructionTree?.Name}.zip"));

					return await Task.FromResult(file);
				}
				catch(Exception exception)
				{
					const string message = "Could not create certificates archive.";

					if(this.Logger.IsEnabled(LogLevel.Error))
						this.Logger.LogError(exception, message);

					this.ModelState.AddModelError("Exception", $"{message} {exception}");
				}
			}

			this.SortModelState();

			return await Task.FromResult(this.View(new JsonConfiguredCertificatesViewModel { Form = form }));
		}

		protected internal virtual void ValidateConstructionTree(CertificateConstructionTreeOptions constructionTree)
		{
			ArgumentNullException.ThrowIfNull(constructionTree);

			if(!constructionTree.Roots.Any())
				throw new InvalidOperationException("There are no root-certificates configured.");
		}

		#endregion
	}
}