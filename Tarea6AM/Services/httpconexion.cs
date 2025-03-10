using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tarea6AMAM.Services
{
    public class httpconexion
    {
        private static readonly HttpClient _httpClient = new(); // Reutilizar HttpClient

        public async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Error en la solicitud: {response.StatusCode}");

                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(jsonResponse))
                    return default; // Retorna null si la respuesta está vacía

                T? result = JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? throw new JsonException("Error: No se pudo deserializar la respuesta.");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error en la solicitud HTTP: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error al procesar JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }
    }
}
