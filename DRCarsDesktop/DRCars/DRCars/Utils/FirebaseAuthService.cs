using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DRCars.Utils
{
    public class FirebaseAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseApiKey;
        private string _currentToken;

        public FirebaseAuthService()
        {
            _httpClient = new HttpClient();
            _firebaseApiKey = AppConfig.FirebaseApiKey;
        }

        public string CurrentToken => _currentToken;
        public bool IsAuthenticated => !string.IsNullOrEmpty(_currentToken);

        /// <summary>
        /// Autentica con Firebase y obtiene el token de acceso
        /// </summary>
        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                Console.WriteLine("=== INICIANDO AUTENTICACIÓN FIREBASE ===");
                Console.WriteLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                Console.WriteLine($"API Key configurada: {(_firebaseApiKey?.Length > 0 ? $"{_firebaseApiKey.Substring(0, 10)}..." : "NO CONFIGURADA")}");
                Console.WriteLine($"API Key completa: {_firebaseApiKey}");

                if (string.IsNullOrEmpty(_firebaseApiKey))
                {
                    Console.WriteLine("❌ ERROR: Firebase API Key no está configurada");
                    return false;
                }

                var authUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseApiKey}";
                Console.WriteLine($"URL de autenticación: {authUrl}");

                var authData = new
                {
                    email = "pruebaimagen@prueba.com",
                    password = "pasw123",
                    returnSecureToken = true
                };

                Console.WriteLine($"Datos de autenticación:");
                Console.WriteLine($"  - Email: {authData.email}");
                Console.WriteLine($"  - Password: {new string('*', authData.password.Length)}");
                Console.WriteLine($"  - Return Secure Token: {authData.returnSecureToken}");

                var json = JsonConvert.SerializeObject(authData);
                Console.WriteLine($"JSON a enviar: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine($"Content-Type: application/json");
                Console.WriteLine($"Encoding: UTF-8");

                // Configurar timeout para la petición
                _httpClient.Timeout = TimeSpan.FromSeconds(30);
                Console.WriteLine($"Timeout configurado: 30 segundos");

                Console.WriteLine("📤 Enviando petición POST a Firebase Auth...");
                var startTime = DateTime.Now;

                var response = await _httpClient.PostAsync(authUrl, content);

                var endTime = DateTime.Now;
                var duration = endTime - startTime;

                Console.WriteLine($"📥 Respuesta recibida en {duration.TotalMilliseconds}ms");
                Console.WriteLine($"Status Code: {(int)response.StatusCode} ({response.StatusCode})");
                Console.WriteLine($"Reason Phrase: {response.ReasonPhrase}");
                Console.WriteLine($"Headers de respuesta:");

                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }

                foreach (var header in response.Content.Headers)
                {
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Tamaño de respuesta: {responseContent.Length} caracteres");
                Console.WriteLine($"Contenido de la respuesta: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ Respuesta HTTP exitosa, procesando JSON...");

                    try
                    {
                        var authResponse = JsonConvert.DeserializeObject<FirebaseAuthResponse>(responseContent);

                        if (authResponse != null)
                        {
                            Console.WriteLine("✅ JSON deserializado correctamente:");
                            Console.WriteLine($"  - Email: {authResponse.Email}");
                            Console.WriteLine($"  - LocalId: {authResponse.LocalId}");
                            Console.WriteLine($"  - Registered: {authResponse.Registered}");
                            Console.WriteLine($"  - ExpiresIn: {authResponse.ExpiresIn} segundos");
                            Console.WriteLine($"  - RefreshToken: {(string.IsNullOrEmpty(authResponse.RefreshToken) ? "NO" : "SÍ")} presente");
                            Console.WriteLine($"  - IdToken: {(string.IsNullOrEmpty(authResponse.IdToken) ? "NO" : "SÍ")} presente");

                            if (!string.IsNullOrEmpty(authResponse.IdToken))
                            {
                                _currentToken = authResponse.IdToken;
                                Console.WriteLine($"🔑 Token JWT obtenido:");
                                Console.WriteLine($"  - Longitud: {_currentToken.Length} caracteres");
                                Console.WriteLine($"  - Primeros 50 caracteres: {_currentToken.Substring(0, Math.Min(50, _currentToken.Length))}...");
                                Console.WriteLine($"  - Últimos 20 caracteres: ...{_currentToken.Substring(Math.Max(0, _currentToken.Length - 20))}");

                                // Intentar decodificar el header del JWT para más info
                                try
                                {
                                    var parts = _currentToken.Split('.');
                                    if (parts.Length >= 2)
                                    {
                                        var header = parts[0];
                                        var payload = parts[1];

                                        // Añadir padding si es necesario
                                        while (header.Length % 4 != 0) header += "=";
                                        while (payload.Length % 4 != 0) payload += "=";

                                        var headerBytes = Convert.FromBase64String(header);
                                        var payloadBytes = Convert.FromBase64String(payload);

                                        var headerJson = Encoding.UTF8.GetString(headerBytes);
                                        var payloadJson = Encoding.UTF8.GetString(payloadBytes);

                                        Console.WriteLine($"🔍 JWT Header: {headerJson}");
                                        Console.WriteLine($"🔍 JWT Payload: {payloadJson}");
                                    }
                                }
                                catch (Exception jwtEx)
                                {
                                    Console.WriteLine($"⚠️ No se pudo decodificar JWT: {jwtEx.Message}");
                                }

                                Console.WriteLine("✅ AUTENTICACIÓN FIREBASE COMPLETADA EXITOSAMENTE");
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("❌ Error: Respuesta exitosa pero IdToken está vacío o nulo");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("❌ Error: authResponse es null después de deserializar");
                            return false;
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"❌ Error al deserializar respuesta JSON:");
                        Console.WriteLine($"  - Mensaje: {jsonEx.Message}");
                        Console.WriteLine($"  - Tipo: {jsonEx.GetType().Name}");
                        Console.WriteLine($"  - StackTrace: {jsonEx.StackTrace}");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"❌ Error en la petición HTTP:");
                    Console.WriteLine($"   Status Code: {(int)response.StatusCode} ({response.StatusCode})");
                    Console.WriteLine($"   Reason: {response.ReasonPhrase}");
                    Console.WriteLine($"   Contenido: {responseContent}");

                    // Intentar parsear el error de Firebase
                    try
                    {
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        if (errorResponse?.error != null)
                        {
                            Console.WriteLine($"🔍 Error detallado de Firebase:");
                            Console.WriteLine($"   - Mensaje: {errorResponse.error.message}");
                            Console.WriteLine($"   - Código: {errorResponse.error.code}");

                            if (errorResponse.error.errors != null)
                            {
                                Console.WriteLine($"   - Errores adicionales:");
                                foreach (var err in errorResponse.error.errors)
                                {
                                    Console.WriteLine($"     * {err.message} (Dominio: {err.domain}, Razón: {err.reason})");
                                }
                            }
                        }
                    }
                    catch (Exception parseEx)
                    {
                        Console.WriteLine($"   ⚠️ No se pudo parsear el error de Firebase: {parseEx.Message}");
                    }

                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"❌ Error de conexión HTTP:");
                Console.WriteLine($"   - Mensaje: {httpEx.Message}");
                Console.WriteLine($"   - Detalles: {httpEx.InnerException?.Message}");
                Console.WriteLine($"   - Tipo: {httpEx.GetType().Name}");
                return false;
            }
            catch (TaskCanceledException tcEx)
            {
                Console.WriteLine($"❌ Timeout en la petición:");
                Console.WriteLine($"   - Mensaje: {tcEx.Message}");
                Console.WriteLine($"   - Cancelado: {tcEx.CancellationToken.IsCancellationRequested}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general durante autenticación Firebase:");
                Console.WriteLine($"   - Mensaje: {ex.Message}");
                Console.WriteLine($"   - Tipo de excepción: {ex.GetType().Name}");
                Console.WriteLine($"   - Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Limpia el token de autenticación
        /// </summary>
        public void ClearToken()
        {
            _currentToken = null;
        }

        /// <summary>
        /// Obtiene el header de autorización para las peticiones
        /// </summary>
        public string GetAuthorizationHeader()
        {
            return IsAuthenticated ? $"Bearer {_currentToken}" : null;
        }
    }

    public class FirebaseAuthResponse
    {
        [JsonProperty("idToken")]
        public string IdToken { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("expiresIn")]
        public string ExpiresIn { get; set; }

        [JsonProperty("localId")]
        public string LocalId { get; set; }

        [JsonProperty("registered")]
        public bool Registered { get; set; }
    }
}
