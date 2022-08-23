using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using Application.Models;
using Application.Models.Cryptography;
using Application.Models.Cryptography.Extensions;
using Application.Models.Views.JsonConfiguredCertificates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class JsonConfiguredCertificatesController : SiteController
	{
		#region Constructors

		public JsonConfiguredCertificatesController(IFacade facade) : base(facade) { }

		#endregion

		#region Methods

		public virtual async Task<IActionResult> Index()
		{
			return await Task.FromResult(this.View(new JsonConfiguredCertificatesViewModel()));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[SuppressMessage("Maintainability", "CA1506:Avoid excessive class coupling")]
		[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
		public virtual async Task<IActionResult> Index(JsonConfiguredCertificatesForm form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

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

					var archive = this.Facade.ArchiveFactory.Create(certificates, form.Password);

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
			if(constructionTree == null)
				throw new ArgumentNullException(nameof(constructionTree));

			if(!constructionTree.Roots.Any())
				throw new InvalidOperationException("There are no root-certificates configured.");
		}

		#endregion
	}
}