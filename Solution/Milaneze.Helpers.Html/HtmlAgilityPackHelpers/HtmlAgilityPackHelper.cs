using HtmlAgilityPack;
using System;

namespace Milaneze.Helpers.Html.HtmlAgilityPackHelpers
{
    public class HtmlAgilityPackHelper
    {
        private static string getTextSemEspacos(string texto)
        {
            if (texto == null)
                return null;

            return HtmlEntity.DeEntitize(texto.Replace("\r", "").Replace("\n", "").Replace("\t", "")).Trim();
        }

        /// <summary>
        /// Pega o texto do HtmlNode cortando os espaços em branco.
        /// </summary>
        public static string GetInnerTextSemEspaco(HtmlNode node)
        {
            if (node == null)
                return null;

            return getTextSemEspacos(node.InnerText);
        }

        /// <summary>
        /// Pega o texto do HtmlNode e retorna como DateTime, caso possível.
        /// </summary>
        public static DateTime? GetInnerTextDateTimeNullableSafe(HtmlNode node)
        {
            string textoRetornado = GetInnerTextSemEspaco(node);

            DateTime retorno;

            if(DateTime.TryParse(textoRetornado, out retorno))
                return retorno;

            return null;
        }

        /// <summary>
        /// Pega o texto do HtmlNode e retorna como DateTime. Caso não seja possível, retorne new DateTime().
        /// </summary>
        public static DateTime GetInnerTextDateTimeSafe(HtmlNode node)
        {
            DateTime? retorno = GetInnerTextDateTimeNullableSafe(node);

            if (retorno.HasValue)
                return retorno.Value;

            return new DateTime();
        }

        /// <summary>
        /// Pega o texto do HtmlNode e retorna como Decimal, caso possível.
        /// </summary>
        public static Decimal? GetInnerTextDecimalNullableSafe(HtmlNode node)
        {
            string textoRetornado = GetInnerTextSemEspaco(node);

            Decimal retorno;

            if (Decimal.TryParse(textoRetornado, out retorno))
                return retorno;

            return null;
        }

        /// <summary>
        /// Pega o texto do HtmlNode e retorna como Decimal. Caso não seja possível, retorne new 0.
        /// </summary>
        public static Decimal GetInnerTextDecimalSafe(HtmlNode node)
        {
            Decimal? retorno = GetInnerTextDecimalNullableSafe(node);

            if (retorno.HasValue)
                return retorno.Value;

            return decimal.Zero;
        }

        /// <summary>
        /// Pega o texto do HtmlNode e retorna como Int32, caso possível.
        /// </summary>
        public static int? GetInnerTextIntNullableSafe(HtmlNode node)
        {
            string textoRetornado = GetInnerTextSemEspaco(node);

            int retorno;

            if (int.TryParse(textoRetornado, out retorno))
                return retorno;

            return null;
        }

        /// <summary>
        /// Pega o texto do HtmlNode e retorna como Int32. Caso não seja possível, retorne new 0.
        /// </summary>
        public static int GetInnerTextIntSafe(HtmlNode node)
        {
            int? retorno = GetInnerTextIntNullableSafe(node);

            if (retorno.HasValue)
                return retorno.Value;

            return 0;
        }
    }
}
