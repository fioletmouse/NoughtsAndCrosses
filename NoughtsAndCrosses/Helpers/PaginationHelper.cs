using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NoughtsAndCrosses.Models;

namespace NoughtsAndCrosses.Helpers
{
    public static class PaginationHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              Pagination pageInfo)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", "#");
                tag.InnerHtml = i.ToString();
                tag.MergeAttribute("onClick", "pageSwitch("+i+")");
                // если текущая страница, то выделяем ее, добавляя класс
                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}