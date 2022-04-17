﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Desglose
{
    //C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\System.Management.dll

    public class ManejadorDatos
    {
        //  private static string url;
        //  private static HttpWebRequest request;
        public ResultDTO resultnh { get; set; }

        private string _url;
        private HttpClient _httpClient;
        private Conterrors _errors;


        public ManejadorDatos()
        {
            _url = $"https://apirestmembresia-nh.herokuapp.com/";
            //_url = $"http://localhost:8080";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_url);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        //a)usuario 
        public bool PostBitacora(string _comando)
        {
            try
            {
                Task.Run(async () => await PostBitacoraAsync(_comando)).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task PostBitacoraAsync(string _comando)
        {
            try
            {

                InfoSystema_validar _InfoSystema = new InfoSystema_validar();
                _InfoSystema.M1_EjecutarInfoSistem();
                _InfoSystema.M3_getMacAddress3();


                BitacoraDTO _usuariosDTO = new BitacoraDTO()
                {
                    idMAC = _InfoSystema.mac,
                    iPUsuario = "cargado",
                    usuario = _InfoSystema.usuario,
                    comando = _comando
                };

                var jsonResul = JsonConvert.SerializeObject(_usuariosDTO);
                HttpContent httpContent = new StringContent(jsonResul, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/bitacora", httpContent);


                if (response.IsSuccessStatusCode)
                {
                    var result = response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    resultnh = JsonConvert.DeserializeObject<ResultDTO>(json);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _errors = JsonConvert.DeserializeObject<Conterrors>(json);
                    resultnh = new ResultDTO()
                    {
                        Isok = false,
                        msg = _errors.errors[0].msg
                    };
                }

            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                resultnh = new ResultDTO()
                {
                    msg = $" error catch ex:{ex.Message}",
                    Isok = false
                };
            }

        }


        public bool GetBitacora()
        {
            try
            {
                Task.Run(async () => await GetBitacoraAsync()).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task GetBitacoraAsync()
        {
            try
            {


                var response = await _httpClient.GetAsync("api/bitacora");
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();

                    var _usuariosDTO = new ListaBitacoraDTO();
                    if (response.Content.Headers.ContentType.MediaType == "application/json")
                    {
                        _usuariosDTO = JsonConvert.DeserializeObject<ListaBitacoraDTO>(json);
                    }
                    else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                    {
                    }
                }
                else
                { }
            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
            }
        }

        //b) inscripcion

        public bool PostInscripcion(string empresa)
        {
            try
            {
                Task.Run(async () => await PostInscripcionASync(empresa)).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task PostInscripcionASync(string empresa)
        {
            try
            {

                InfoSystema_validar _InfoSystema = new InfoSystema_validar();
                _InfoSystema.Ejecutar();

                InscripcionDTO _inscripcionDTO = new InscripcionDTO()
                {
                    idMAC = _InfoSystema.mac,
                    iPUsuario = _InfoSystema.IpPublic,
                    usuario = _InfoSystema.usuario,
                    EmpresaEjecutable = empresa
                };


                var jsonResul = JsonConvert.SerializeObject(_inscripcionDTO);
                HttpContent httpContent = new StringContent(jsonResul, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/inscripcion", httpContent);



                if (response.IsSuccessStatusCode)
                {
                    var result = response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    resultnh = JsonConvert.DeserializeObject<ResultDTO>(json);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        resultnh = JsonConvert.DeserializeObject<ResultDTO>(json);
                    }
                    else
                    {
                        _errors = JsonConvert.DeserializeObject<Conterrors>(json);
                        resultnh = new ResultDTO()
                        {
                            Isok = (_errors.errors[0].msg.Contains("idMAC existe") ? true : false),
                            msg = _errors.errors[0].msg
                        };
                    }

                }

            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                resultnh = new ResultDTO()
                {
                    msg = $" error catch ex:{ex.Message}",
                    Isok = false
                };
            }

        }

        public bool GetInscripcion()
        {
            try
            {
                Task.Run(async () => await GetInscripcionASync()).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task GetInscripcionASync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/inscripcion");
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();

                    var _ListaInscripcionDTO = new ListaInscripcionDTO();
                    if (response.Content.Headers.ContentType.MediaType == "application/json")
                    {
                        _ListaInscripcionDTO = JsonConvert.DeserializeObject<ListaInscripcionDTO>(json);
                    }
                    else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                    {
                    }
                }
                else
                { }
            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
            }
        }
    }

    //moel
    #region model

    public class Conterrors
    {
        public errors[] errors { get; set; }

    }

    public class errors
    {
        public string value { get; set; }
        public string msg { get; set; }
        public string param { get; set; }
        public string location { get; set; }
    }

    public class ResultDTO
    {
        public string msg { get; set; }
        public bool Isok { get; set; }
        public ResultDTO()
        {
            msg = "conexion fallida";
            Isok = false;
        }
    }

    public class ListaBitacoraDTO
    {
        public string msg { get; set; }
        public List<BitacoraDTO> bitacoras { get; set; }

    }

    public class BitacoraDTO
    {
        public string idMAC { get; set; }
        public string iPUsuario { get; set; }
        public string usuario { get; set; }
        public string comando { get; set; }
        public bool Isok { get; set; }
        public int __v { get; set; }
        public string _id { get; set; }

    }

    public class ListaInscripcionDTO
    {
        public string msg { get; set; }
        public List<InscripcionDTO> Inscripcions { get; set; }

    }

    public class InscripcionDTO
    {

        public string idMAC { get; set; }
        public string iPUsuario { get; set; }
        public string usuario { get; set; }
        public string EmpresaEjecutable { get; set; }
        public bool IsPermitido { get; set; }
        public int __v { get; set; }
        public string _id { get; set; }

    }

    public class InfoSystema_validar
    {
        #region 0)propieades
        public string disco { get; set; }
        public string mac { get; set; } //realmente es la direccion o numero de la placa madre
        public string IpPublic { get; set; }
        public string usuario { get; set; }
        public string ruta { get; set; }
        public string caso { get; set; }

        #endregion

        #region 1)Contructor
        public InfoSystema_validar() { }
        public InfoSystema_validar(string ruta, string caso)
        {
            this.ruta = ruta;
            this.caso = caso;

        }

        public bool Ejecutar()
        {
            try
            {

                if (!M1_EjecutarInfoSistem()) return false;
                if (!M2_ObtenerIp()) return false;
                if (!M3_getMacAddress3()) return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en 'InfoSystema2'" + ex.Message);
                return false; ;
            }
            return true;
        }


        #endregion

        #region Metodos Sitema

        public bool M1_EjecutarInfoSistem()
        {

            try
            {
                // OBTENER INFO DISCO DURO
                string consultaSQLArquitectura = "SELECT * FROM Win32_Processor";
                ManagementObjectSearcher objArquitectura = new ManagementObjectSearcher(consultaSQLArquitectura);
                ManagementObject serialDD = new ManagementObject(@"Win32_PhysicalMedia='\\.\PHYSICALDRIVE0'");

                foreach (PropertyData prop in serialDD.Properties)
                {
                    Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
                }

                disco = serialDD.GetPropertyValue("SerialNumber").ToString();
                usuario = Environment.UserName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en 'M1_EjecutarInfoSistem'" + ex.Message);
                return false; ;
            }
            return true;

        }
        public bool M2_ObtenerIp()
        {
            try
            {
                string ExternalIP;
                ExternalIP = new WebClient().DownloadString("http://checkip.dyndns.org/");
                IpPublic = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Matches(ExternalIP)[0].ToString();
                Console.WriteLine("Ippublica:{0}", IpPublic);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en 'M2_obtenerIp'" + ex.Message);
                return false; ;
            }
            return true;
        }

        public bool M3_GetMacAddress()
        {
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Only consider Ethernet network interfaces
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up)
                    {
                        PhysicalAddress _PhysicalAddress = nic.GetPhysicalAddress();
                        if (_PhysicalAddress != null)
                        {
                            mac = _PhysicalAddress.ToString();
                            Console.WriteLine("mac:{0}", mac);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en 'M3_GetMacAddress'" + ex.Message);
                return false; ;
            }
            return true;
        }

        // problemna de la mac (numero de tarjeta de red conectada)es que varia segun el dispositovo q este conectado
        //cable o wifi -- entonces estenumero varia
        public bool M3_GetMacAddress2()
        {

            try
            {

                M3_getMacAddress3();

                const int MIN_MAC_ADDR_LENGTH = 12;
                string macAddress = string.Empty;
                long maxSpeed = -1;

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    Debug.WriteLine("Found MAC Address: " + nic.GetPhysicalAddress() + " Type: " + nic.NetworkInterfaceType);

                    string tempMac = nic.GetPhysicalAddress().ToString();
                    string nombre = nic.Name;
                    if (nic.Speed > maxSpeed &&
                        !string.IsNullOrEmpty(tempMac) &&
                        tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                    {
                        Debug.WriteLine("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                        maxSpeed = nic.Speed;
                        mac = tempMac;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" error en 'M3_GetMacAddress'" + ex.Message);
                return false; ;
            }
            return true;

        }


        public bool M3_getMacAddress3()
        {
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                foreach (ManagementObject getserial in MOS.Get())
                {
                    mac = getserial["SerialNumber"].ToString();  // realmente placa madre
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        #endregion

    }

    #endregion
}
