using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Skybrud.Integrations.BorgerDk.WebService;

namespace Limbo.Integrations.BorgerDk {

    public class BorgerDkEndpoint {

        #region Member properties

        /// <summary>
        /// Gets the domain of the endpoint.
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Gets a readable name for the endpoint.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the sub folder for the endpoint. The name of sub folder is
        /// language specific - eg. "Sider" for Danish or "Pages" for English.
        /// </summary>
        public string SubFolder { get; }
        
        /// <summary>
        /// Gets the full URL to the webservice endpoint.
        /// </summary>
        public string EndpointUrl { get; }

        #endregion

        #region Static properties

        /// <summary>
        /// Gets the endpoint for "www.borger.dk".
        /// </summary>
        public static readonly BorgerDkEndpoint Default = new BorgerDkEndpoint(
            "www.borger.dk", "Borger.dk", "Sider", "https://www.borger.dk/_vti_bin/borger/ArticleExport.svc"
        );

        /// <summary>
        /// Gets the endpoint for "lifeindenmark.borger.dk".
        /// </summary>
        public static readonly BorgerDkEndpoint LifeInDenmark = new BorgerDkEndpoint(
            "lifeindenmark.borger.dk", "Life in Denmark", "Pages", "https://lifeindenmark.borger.dk/_vti_bin/borger/ArticleExport.svc"
        );

        /// <summary>
        /// Gets an array of all known endpoints.
        /// </summary>
        public static BorgerDkEndpoint[] Values => new [] { Default, LifeInDenmark };

        #endregion

        #region Constructor

        private BorgerDkEndpoint(string domain, string name, string subFolder, string endpoint) {
            Domain = domain;
            Name = name;
            SubFolder = subFolder;
            EndpointUrl = endpoint;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Gets the export client for the domain.
        /// </summary>
        public ArticleExportClient GetClient() {
            return new ArticleExportClient(GetDefaultBinding(), new EndpointAddress(EndpointUrl));
        }

        /// <summary>
        /// Gets whether the specified URL is valid in relation to the domain.
        /// </summary>
        /// <param name="url">The URL to validate.</param>
        public bool IsValidUrl(string url) {
            
            // Obviously an empty URL is not valid
            if (string.IsNullOrWhiteSpace(url)) return false;
            url = url.Split('?')[0];
            
            // In December 2016, Borger.dk moved to a new CMS that uses a
            // different URL structure. This means that we can't really tell
            // whether an URL refers to an article or just a normal page. So
            // for now, we just validate the domain.
            return url.StartsWith("https://" + Domain + "/");
        
        }

        #endregion

        #region Static methods

#if NET_FRAMEWORK

        /// <summary>
        /// Gets the default binding used to communicate with the Borger.dk webservices.
        /// </summary>
        public static Binding GetDefaultBinding() {
            return new BasicHttpBinding {
                CloseTimeout = TimeSpan.FromMinutes(1),
                OpenTimeout = TimeSpan.FromMinutes(1),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                SendTimeout = TimeSpan.FromMinutes(1),
                AllowCookies = false,
                BypassProxyOnLocal = false,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                MaxBufferSize = 1048576, // 1 MB
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 1048576, // 1 MB
                MessageEncoding = WSMessageEncoding.Text,
                TextEncoding = Encoding.UTF8,
                TransferMode = TransferMode.Buffered,
                UseDefaultWebProxy = true,
                ReaderQuotas = new XmlDictionaryReaderQuotas {
                    MaxDepth = 32,
                    MaxStringContentLength = 65536,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                },
                Security = new BasicHttpSecurity {
                    Mode = BasicHttpSecurityMode.Transport,
                    Transport = new HttpTransportSecurity {
                        ClientCredentialType = HttpClientCredentialType.None,
                        ProxyCredentialType = HttpProxyCredentialType.None,
                        Realm = ""
                    },
                    Message = new BasicHttpMessageSecurity {
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName,
                        AlgorithmSuite = SecurityAlgorithmSuite.Default
                    }
                }
            };
        }

#else
        /// <summary>
        /// Gets the default binding used to communicate with the Borger.dk webservices.
        /// </summary>
        public static Binding GetDefaultBinding() {
            return new BasicHttpBinding {
                CloseTimeout = TimeSpan.FromMinutes(1),
                OpenTimeout = TimeSpan.FromMinutes(1),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                SendTimeout = TimeSpan.FromMinutes(1),
                AllowCookies = false,
                BypassProxyOnLocal = false,
                MaxBufferSize = 1048576, // 1 MB
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 1048576, // 1 MB
                TextEncoding = Encoding.UTF8,
                TransferMode = TransferMode.Buffered,
                UseDefaultWebProxy = true,
                ReaderQuotas = new XmlDictionaryReaderQuotas {
                    MaxDepth = 32,
                    MaxStringContentLength = 65536,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                },
                Security = new BasicHttpSecurity {
                    Mode = BasicHttpSecurityMode.Transport,
                    Transport = new HttpTransportSecurity {
                        ClientCredentialType = HttpClientCredentialType.None,
                        ProxyCredentialType = HttpProxyCredentialType.None
                    },
                    Message = new BasicHttpMessageSecurity {
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName
                    }
                }
            };
        }

#endif

        /// <summary>
        /// Gets the endpoint from the specified URL. If no matching endpoint can be
        /// found, NULL will be returned.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns></returns>
        public static BorgerDkEndpoint GetFromUrl(string url) {
            return GetFromDomain(Regex.Match(url ?? string.Empty, "^https://([a-z]+\\.borger\\.dk)/").Groups[1].Value);
        }

        /// <summary>
        /// Gets the endpoint from the specified domain. If no matching endpoint can be
        /// found, NULL will be returned.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <returns></returns>
        public static BorgerDkEndpoint GetFromDomain(string domain) {
            return Values.FirstOrDefault(d => d.Domain == domain);
        }

        #endregion

    }

}