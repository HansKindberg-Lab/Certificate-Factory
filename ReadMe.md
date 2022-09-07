# Certificate-Factory

This is a web-application for creating certificates. **Important**: only certificates for development, testing and laborating.

![.github/workflows/Docker-deploy.yml](https://github.com/HansKindberg-Lab/Certificate-Factory/actions/workflows/Docker-deploy.yml/badge.svg)

Web-application, without configuration, pushed to Docker Hub: https://hub.docker.com/r/hanskindberg/certificate-factory

## Configuration

Deploy this application on https only. There is sensitive information in the certificate zip-file that gets downloaded.

If your deployment-environment use TLS-termination and you need forwarded headers:

	{
		"ForwardedHeaders": {
			"ForwardedHeaders": "All"
		}
	}

You can configure more ForwardedHeaders-options:

	{
		"ForwardedHeaders": {
			"AllowedHosts": [
				"example.com"
			],
			"ForwardedHeaders": "All",
			"KnownNetworks": [
				"127.0.0.1/8"
			],
			"KnownProxies": [
				"::1"
			]
		}
	}

- [ForwardedHeadersOptions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.forwardedheadersoptions)
- To make ForwardedHeadersOptions configurable in appsettings.json: [/Source/Application/Models/ComponentModel](/Source/Application/Models/ComponentModel)
- [KnownNetworks](https://www.ipaddressguide.com/cidr)

## Certificate-store

The application contains a certificate-store. Here we can put root-certificates and intermediate-certificates to use for signing the certificates we create. The certificates in the certificate-store are configured in appsettings.json. Example:

- [appsettings.Development.json](/Source/Application/appsettings.Development.json#L2)

The certificates configured for the development-environment are:

### ECDsa

- **Certificate-Factory | Unsecure Root CA | ECDsa** - created with asymmetric algorithm *ECDsa - BrainpoolP512R1*
- **Certificate-Factory | Unsecure Intermediate CA | ECDsa** - created with asymmetric algorithm *ECDsa - BrainpoolP512R1*

### RSA

- **Certificate-Factory | Unsecure Root CA | RSA** - created with asymmetric algorithm *RSA - 2048 - Pkcs1*
- **Certificate-Factory | Unsecure Intermediate CA | RSA** - created with asymmetric algorithm *RSA - 2048 - Pkcs1*

### Important

**Do not trust these certificates. They are only here to be able to run this application in development and create issued certificates.**

## Client-certificate

When we create a client-certificate we may want to create a more "complex" subject containing information about a person for example. There are "rules" for what this distinguished name can contain.

Google: X509 cn sn serialnumber c l st ou

- [Distinguished Names](https://www.ibm.com/docs/en/ibm-mq/7.5?topic=certificates-distinguished-names)

	- **C**, Country
	- **CN**, Common Name
	- **DC**, Domain component
	- **DNQ**, Distinguished name qualifier
	- **E**, Email address (Deprecated in preference to MAIL)
	- **L**, Locality name
	- **MAIL**, Email address
	- **O**, Organization name
	- **OU**, Organizational Unit name
	- **PC**, Postal code / zip code
	- **SERIALNUMBER**, Certificate serial number
	- **ST** or **SP** or **S**, State or Province name
	- **STREET**, Street / First line of address
	- **UID** or **USERID**, User identifier
	- **T**, Title
	- **UNSTRUCTUREDADDRESS**, IP address
	- **UNSTRUCTUREDNAME**, Host name

- [Specifying Distinguished Names](https://www.cryptosys.net/pki/manpki/pki_distnames.html)

	- **C**, countryName, 2.5.4.6
	- **CN**, commonName, 2.5.4.3
	- **DC**, domainComponent, 0.9.2342.19200300.100.1.25
	- **E**, emailAddress (deprecated), 1.2.840.113549.1.9.1
	- **G** or **GN**, givenName, 2.5.4.42
	- **L**, localityName, 2.5.4.7
	- **O**, organizationName, 2.5.4.10
	- **OU**, organizationalUnit, 2.5.4.11
	- **SERIALNUMBER**, serialNumber, 2.5.4.5
	- **SN**, surname, 2.5.4.4
	- **ST** or **S**, stateOrProvinceName, 2.5.4.8
	- **STREET**, streetAddress, 2.5.4.9
	- **T** or **TITLE**, title, 2.5.4.12
	- **UID**, userID, 0.9.2342.19200300.100.1.1

## Creating certificates from json-configuration files

We can create certificates from json-configuration files. File example:

	{
		"Defaults": {
			"HashAlgorithm": "Sha256",
			"NotAfter": "2050-01-01"
		},
		"Roots": {
			"root-certificate": {
				"Certificate": {
					"Subject": "CN=Test Root CA"
				},
				"IssuedCertificates": {
					"https-certificate": {
						"Certificate": {
							"EnhancedKeyUsage": "ServerAuthentication",
							"KeyUsage": "DigitalSignature",
							"Subject": "CN=Test Https certificate",
							"SubjectAlternativeName": {
								"DnsNames": [
									"localhost"
								]
							}
						}
					},
					"intermediate-certificate-1": {
						"Certificate": {
							"CertificateAuthority": {
								"PathLengthConstraint": 0
							},
							"KeyUsage": "KeyCertSign",
							"Subject": "CN=Test Intermediate 1"
						},
						"IssuedCertificates": {
							"client-certificate-1": {
								"Certificate": {
									"EnhancedKeyUsage": "ClientAuthentication",
									"KeyUsage": "DigitalSignature",
									"Subject": "CN=Test Client certificate 1"
								}
							}
						}
					},
					"intermediate-certificate-2": {
						"Certificate": {
							"CertificateAuthority": {
								"PathLengthConstraint": 0
							},
							"KeyUsage": "KeyCertSign",
							"Subject": "CN=Test Intermediate 2"
						},
						"IssuedCertificates": {
							"client-certificate-2": {
								"Certificate": {
									"EnhancedKeyUsage": "ClientAuthentication",
									"KeyUsage": "DigitalSignature",
									"Subject": "CN=Test Client certificate 2"
								}
							}
						}
					},
					"intermediate-certificate-3": {
						"Certificate": {
							"CertificateAuthority": {
								"PathLengthConstraint": 0
							},
							"KeyUsage": "KeyCertSign",
							"Subject": "CN=Test Intermediate 3"
						},
						"IssuedCertificates": {
							"client-certificate-3": {
								"Certificate": {
									"EnhancedKeyUsage": "ClientAuthentication",
									"KeyUsage": "DigitalSignature",
									"Subject": "CN=Test Client certificate 3"
								}
							}
						}
					},
					"intermediate-certificate-4": {
						"Certificate": {
							"CertificateAuthority": {
								"PathLengthConstraint": 0
							},
							"KeyUsage": "KeyCertSign",
							"Subject": "CN=Test Intermediate 4"
						},
						"IssuedCertificates": {
							"client-certificate-4": {
								"Certificate": {
									"EnhancedKeyUsage": "ClientAuthentication",
									"KeyUsage": "DigitalSignature",
									"Subject": "CN=Test Client certificate 4"
								}
							}
						}
					}
				}
			}
		},
		"RootsDefaults": {
			"CertificateAuthority": {
				"PathLengthConstraint": null
			},
			"KeyUsage": "KeyCertSign"
		}
	}

## Cryptography

- **Diffie-Hellman** (1977)
- **DSA** - **D**igital **S**ignature **A**lgorithm (1991)
- **ECC** - **E**lliptic **C**urve **C**ryptography (1985, 2005)
- **ECDSA** - **E**lliptic **C**urve **D**igital **S**ignature **A**lgorithm (1985, 2005)
	- Based on DSA + ECC
- **RSA** - **R**ivest **S**hamir **A**dleman (1983)

- [Diffie-Hellman, RSA, DSA, ECC and ECDSA – Asymmetric Key Algorithms](https://www.ssl2buy.com/wiki/diffie-hellman-rsa-dsa-ecc-and-ecdsa-asymmetric-key-algorithms)

## Hash-algorithms

At the moment this application supports:

- SHA256
- SHA384
- SHA512

Creating certificates with the following throws exception:

- MD5
- SHA1

The explanations for this:

*Due to collision problems with MD5 and SHA1, Microsoft recommends a security model based on SHA256 or better.*

- [HashAlgorithmName Struct](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithmname#remarks)

*This method does not support using MD5 or SHA-1 as the hash algorithm for the certificate signature. If you need an MD5 or SHA-1 based certificate signature, you need to implement a custom X509SignatureGenerator and call Create(X500DistinguishedName, X509SignatureGenerator, DateTimeOffset, DateTimeOffset, Byte[]).*

- [CertificateRequest.Create Method](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.certificaterequest.create)
- [CertificateRequest.CreateSelfSigned(DateTimeOffset, DateTimeOffset) Method](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.certificaterequest.createselfsigned)

## Links

- [Create and add certificate to certificate store in .NET 5](https://charlehsin.github.io/coding/dotnet5/2021/11/18/create-correct-cert-for-store-in-net5.html)
- [PEM Loading in .NET Core and .NET 5](https://www.scottbrady91.com/c-sharp/pem-loading-in-dotnet-core-and-dotnet)
- [Cryptography Improvements in .NET 5 - Support for PEM](https://www.tpeczek.com/2020/12/cryptography-improvements-in-net-5.html)
- [Certificate Creation API #20887](https://github.com/dotnet/runtime/issues/20887)
- [API Proposal: Import keys from RFC7468 PEM #31201](https://github.com/dotnet/runtime/issues/31201)
- [Export private/public keys from X509 certificate to PEM](https://stackoverflow.com/questions/43928064/export-private-public-keys-from-x509-certificate-to-pem)
- [Question: how to generate a .pem file with private key from an X509Certificate2? #51597](https://github.com/dotnet/runtime/issues/51597)
- [Create ZIP files on HTTP request without intermediate files using ASP.NET MVC, Razor Pages, and endpoints](https://swimburger.net/blog/dotnet/create-zip-files-on-http-request-without-intermediate-files-using-aspdotnet-mvc-razor-pages-and-endpoints)
- [Distinguished Names](https://www.ibm.com/docs/en/ibm-mq/7.5?topic=certificates-distinguished-names)
- [Specifying Distinguished Names](https://www.cryptosys.net/pki/manpki/pki_distnames.html)
- Google: valid rsa key sizes
- [Size considerations for public and private keys](https://www.ibm.com/docs/en/zos/2.2.0?topic=certificates-size-considerations-public-private-keys)