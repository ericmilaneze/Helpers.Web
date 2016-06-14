using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Milaneze.Helpers.Html.HtmlAgilityPackHelpers;
using System.Text.RegularExpressions;

namespace Milaneze.Helpers.Html.HtmlAgilityPackExtensionMethods
{
    public static class HtmlAgilityPackExtensions
    {
        /// <summary>
        /// Próximo sibling, não do tipo HtmlNodeType.Text ou HtmlNodeType.Comment.
        /// </summary>
        public static HtmlNode NextSiblingNotText(this HtmlNode htmlNode)
        {
            if (htmlNode != null)
            {
                HtmlNode htmlNodeAtual = htmlNode.NextSibling;

                while (true)
                {
                    if (htmlNodeAtual == null)
                        return null;

                    if (htmlNodeAtual.NodeType == HtmlNodeType.Text || htmlNodeAtual.NodeType == HtmlNodeType.Comment)
                        htmlNodeAtual = htmlNodeAtual.NextSibling;
                    else
                        return htmlNodeAtual;
                }
            }

            return null;
        }

        /// <summary>
        /// Próximo sibling, não do tipo HtmlNodeType.Text ou HtmlNodeType.Comment.
        /// </summary>
        /// <param name="quantidadeProximos">Quantidade de nós à frente para navegar.</param>
        public static HtmlNode NextSiblingNotText(this HtmlNode htmlNode, int quantidadeProximos)
        {
            if (htmlNode != null)
            {
                HtmlNode htmlNodeAtual = htmlNode;

                for (int i = 0; i < quantidadeProximos; i++)
                    htmlNodeAtual = NextSiblingNotText(htmlNodeAtual);

                return htmlNodeAtual;
            }

            return null;
        }

        /// <summary>
        /// Capturar texto interno sem espaços.
        /// </summary>
        public static string GetInnerTextSemEspacos(this HtmlNode nodeHtml)
        {
            if (nodeHtml != null)
                return HtmlAgilityPackHelper.GetInnerTextSemEspaco(nodeHtml);

            return null;
        }

        /// <summary>
        /// Retorna o mesmo objeto HtmlDocument, porém sem os comentários.
        /// </summary>
        public static HtmlDocument GetHtmlSemComentarios(this HtmlDocument htmlDocument)
        {
            string htmlSemComentarios = Regex.Replace(htmlDocument.DocumentNode.InnerHtml, "<!--(.*?)-->", string.Empty, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            HtmlDocument documentNovo = new HtmlDocument();
            documentNovo.LoadHtml(htmlSemComentarios);

            return documentNovo;
        }

        /// <summary>
        /// Captura o value do input passado como parâmetro.
        /// </summary>
        /// <param name="inputName">Nome do controle de input.</param>
        public static string GetInputValue(this HtmlDocument htmlDocument, string inputName)
        {
            return htmlDocument.GetAttributeValue(inputName, "input", "value");
        }

        /// <summary>
        /// Captura o value de um atributo de um nó HTML.
        /// </summary>
        /// <param name="nodeName">Nome do nó que terá o atributo capturado.</param>
        /// <param name="nodeType">Tipo do nó.</param>
        /// <param name="attributeName">Atributo.</param>
        public static string GetAttributeValue(this HtmlDocument htmlDocument, string nodeName, string nodeType, string attributeName)
        {
            var node = htmlDocument.FindNodeByName(nodeName, nodeType);

            if (node == null)
                return string.Empty;

            return GetAttributeValue(node, attributeName);
        }

        /// <summary>
        /// Encontra um nó pelo nome e tipo.
        /// </summary>
        /// <param name="name">Nome do nó.</param>
        /// <param name="type">Tipo do nó.</param>
        public static HtmlNode FindNodeByName(this HtmlDocument htmlDocument, string name, string type)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            var elements = htmlDocument.DocumentNode.Descendants().Where(n => !String.IsNullOrWhiteSpace(GetAttributeValue(n, "name")));

            if (!String.IsNullOrWhiteSpace(type))
                elements = elements.Where(n => n.Name.ToUpper() == type.ToUpper());

            return elements.Where(n => GetAttributeValue(n, "name").ToUpper() == name.ToUpper()).FirstOrDefault();
        }

        /// <summary>
        /// Encontra um nó pelo nome.
        /// </summary>
        /// <param name="name">Nome do nó.</param>
        public static HtmlNode FindNodeByName(this HtmlDocument htmlDocument, string name)
        {
            return htmlDocument.FindNodeByName(name, String.Empty);
        }

        /// <summary>
        /// Captura o value de um atributo de um nó HTML.
        /// </summary>
        /// <param name="attributeName">Atributo.</param>
        public static string GetAttributeValue(this HtmlNode node, string attributeName)
        {
            if (node == null || String.IsNullOrWhiteSpace(attributeName) || (!node.Attributes.Contains(attributeName)))
                return String.Empty;

            return node.Attributes[attributeName].Value;
        }

        /// <summary>
        /// Captura um texto dentro de um nó de tabela.
        /// </summary>
        /// <param name="linha">Linha da tabela.</param>
        /// <param name="coluna">Coluna da tabela.</param>
        public static string GetTextoTabela(this HtmlNode tabela, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextSemEspaco(GetTD(tabela, linha, coluna));
        }

        /// <summary>
        /// Captura um texto dentro de uma coleção de linhas: "<tr></tr>".
        /// </summary>
        /// <param name="linha">Linha da coleção.</param>
        /// <param name="coluna">Coluna da coleção.</param>
        public static string GetTextoTabela(this IEnumerable<HtmlNode> linhas, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextSemEspaco(GetTD(linhas, linha, coluna));
        }

        /// <summary>
        /// Captura um Decimal dentro de uma coleção de linhas: "<tr></tr>".
        /// </summary>
        /// <param name="linha">Linha da coleção.</param>
        /// <param name="coluna">Coluna da coleção.</param>
        public static decimal GetDecimalTabela(this IEnumerable<HtmlNode> linhas, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextDecimalSafe(GetTD(linhas, linha, coluna));
        }

        /// <summary>
        /// Captura um Decimal dentro de um nó de tabela.
        /// </summary>
        /// <param name="linha">Linha da tabela.</param>
        /// <param name="coluna">Coluna da tabela.</param>
        public static decimal GetDecimalTabela(this HtmlNode tabela, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextDecimalSafe(GetTD(tabela, linha, coluna));
        }

        /// <summary>
        /// Captura um Int32 dentro de uma coleção de linhas: "<tr></tr>".
        /// </summary>
        /// <param name="linha">Linha da coleção.</param>
        /// <param name="coluna">Coluna da coleção.</param>
        public static int GetIntTabela(this IEnumerable<HtmlNode> linhas, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextIntSafe(GetTD(linhas, linha, coluna));
        }

        /// <summary>
        /// Captura um Int32 dentro de um nó de tabela.
        /// </summary>
        /// <param name="linha">Linha da tabela.</param>
        /// <param name="coluna">Coluna da tabela.</param>
        public static int GetIntTabela(this HtmlNode tabela, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextIntSafe(GetTD(tabela, linha, coluna));
        }

        /// <summary>
        /// Captura um DateTime dentro de um nó de tabela.
        /// </summary>
        /// <param name="linha">Linha da tabela.</param>
        /// <param name="coluna">Coluna da tabela.</param>
        public static DateTime GetDateTimeTabela(this HtmlNode tabela, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextDateTimeSafe(GetTD(tabela, linha, coluna));
        }

        /// <summary>
        /// Captura um DateTime dentro de uma coleção de linhas: "<tr></tr>".
        /// </summary>
        /// <param name="linha">Linha da coleção.</param>
        /// <param name="coluna">Coluna da coleção.</param>
        public static DateTime GetDateTimeTabela(this IEnumerable<HtmlNode> linhas, int linha, int coluna)
        {
            return HtmlAgilityPackHelper.GetInnerTextDateTimeSafe(GetTD(linhas, linha, coluna));
        }

        /// <summary>
        /// Captura um HtmlNode "<td></td>" dentro de uma coleção de linhas: "<tr></tr>.
        /// </summary>
        /// <param name="linha">Linha da tabela.</param>
        /// <param name="coluna">Coluna da tabela.</param>
        public static HtmlNode GetTD(this IEnumerable<HtmlNode> linhas, int linha, int coluna)
        {
            HtmlNode linhaSelecionada = linhas.ElementAtOrDefault(linha);

            if (linhaSelecionada != null)
                return linhaSelecionada.Descendants("td").ElementAtOrDefault(coluna);

            return null;
        }

        /// <summary>
        /// Captura um HtmlNode "<td></td>" dentro de nó de tabela.
        /// </summary>
        /// <param name="linha">Linha da tabela.</param>
        /// <param name="coluna">Coluna da tabela.</param>
        public static HtmlNode GetTD(this HtmlNode tabela, int linha, int coluna)
        {
            if (tabela.TabelaTemLinhas())
                return GetTD(tabela.Descendants("tr").Cast<HtmlNode>(), linha, coluna);

            return null;
        }

        /// <summary>
        /// Verifica se o nó é uma tabela e se tem linhas.
        /// </summary>
        public static bool TabelaTemLinhas(this HtmlNode tabela)
        {
            if (tabela.Name == "table")
                if (tabela.Descendants("tr").Cast<HtmlNode>().Any())
                    return true;

            return false;
        }
    }
}