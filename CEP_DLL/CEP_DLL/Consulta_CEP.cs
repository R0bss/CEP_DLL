using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;
namespace CEP_DLL
{
    public class Consulta_CEP
    {
        public Consulta_CEP() { }
        public string Consulta_api(string cep)
        {
            
            if (cep.Length == 8)
            {
                try
                {
                    RestClient restClient = new RestClient(string.Format("https://viacep.com.br/ws/{0}/json/", cep));
                    RestRequest restRequest = new RestRequest(Method.GET);

                    IRestResponse restResponse = restClient.Execute(restRequest);

                    if (restResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        //MessageBox.Show("Houve um problema com sua requisição: " + restResponse.Content, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return "MsgBox:Houve um problema com sua requisição: " + restResponse.Content;
                    else
                    {
                        DadosRetorno dadosRetorno = new JsonSerializer().Deserialize<DadosRetorno>(restResponse);

                        if (dadosRetorno.cep is null)
                        {
                            //MessageBox.Show("CEP não encontrado!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return "MsgBox:CEP não encontrado!";
                        }

                        //string txtCep = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.cep);
                        //string txtEndereco = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.logradouro);
                        //string txtComp = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.complemento);
                        //string txtBairro = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.bairro);
                        //string txtLocalidade = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.localidade);
                        //string txtUF = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.uf);
                        //string txtUnid = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.unidade);
                        //string txtIBGE = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.ibge);
                        //string txtGIA = ObterStringSemAcentosECaracteresEspeciais(dadosRetorno.gia);

                        string txtCep = dadosRetorno.cep;
                        string txtEndereco = dadosRetorno.logradouro;
                        string txtComp = dadosRetorno.complemento;
                        string txtBairro = dadosRetorno.bairro;
                        string txtLocalidade = dadosRetorno.localidade;
                        string txtUF = dadosRetorno.uf;
                        string txtUnid = dadosRetorno.unidade;
                        string txtIBGE = dadosRetorno.ibge;
                        string txtGIA = dadosRetorno.gia;


                        // Devolução da Consulta de CEP
                        //Create_txt(txtcep, txtEndereco, txtComp, txtBairro, txtLocalidade, txtUF, txtUnid, txtIBGE, txtGIA, arquivotxt);
                        return txtCep + ";" + txtEndereco + ";" + txtComp + ";" + txtBairro + ";" + txtLocalidade + ";" + txtUF + ";" + txtUnid + ";" + txtIBGE + ";" + txtGIA;
                    }
                }
                catch (Exception erro)
                {
                    //MessageBox.Show("Erro geral ao consulta a API: " + erro.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "MsgBox:Erro geral ao consulta a API: " + erro.Message;
                }
            }
            else
            {
                //MessageBox.Show("Formato do CEP invalido, o CEP deve possuir 8 caracteres!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "MsgBox:Formato do CEP invalido, o CEP deve possuir 8 caracteres!";
            }

        }


        class DadosRetorno
        {
            public string cep { get; set; }
            public string logradouro { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string localidade { get; set; }
            public string uf { get; set; }
            public string unidade { get; set; }
            public string ibge { get; set; }
            public string gia { get; set; }

        }
        //Metodo utilizado na versão exe 
        public void Create_txt(string cep, string logradouro, string complemento, string bairro, string localidade, string uf, string unidade, string ibge, string gia, string arquivotxt)
        {
            string destino = GetIni.GetIniValue("BANCO DE DADOS", "PATH", "QIClass.ini");

            //StreamWriter escritor = new StreamWriter(destino + "Textos\\ConsultaCEP_ViaCEP.txt");
            StreamWriter escritor = new StreamWriter(destino + "Textos\\" + arquivotxt);


            escritor.WriteLine("CEP;ENDERECO;COMPLEMENTO;BAIRRO;LOCALIDADE;UF;UNIDADE;IBGE;GIA");
            escritor.WriteLine(cep + ";" + logradouro + ";" + complemento + ";" + bairro + ";" + localidade + ";" + uf + ";" + unidade + ";" + ibge + ";" + gia);

            escritor.Close();

        }
        public static string ObterStringSemAcentosECaracteresEspeciais(string str)
        {
            if (str is null)
            {
                return str;
            }
            else
            {
                /** Troca os caracteres acentuados por não acentuados **/
                string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
                string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

                for (int i = 0; i < acentos.Length; i++)
                {
                    str = str.Replace(acentos[i], semAcento[i]);
                }
                /** Troca os caracteres especiais da string por "" **/
                string[] caracteresEspeciais = { "¹", "²", "³", "£", "¢", "¬", "º", "¨", "\"", "'", ".", ",", "-", ":", "(", ")", "ª", "|", "\\\\", "°", "_", "@", "#", "!", "$", "%", "&", "*", ";", "/", "<", ">", "?", "[", "]", "{", "}", "=", "+", "§", "´", "`", "^", "~" };

                for (int i = 0; i < caracteresEspeciais.Length; i++)
                {
                    str = str.Replace(caracteresEspeciais[i], "");
                }

                /** Troca os caracteres especiais da string por " " **/
                str = Regex.Replace(str, @"[^\w\.@-]", " ",
                                    RegexOptions.None, TimeSpan.FromSeconds(1.5));

                return str.Trim();
            }
        }

    }
}
