
using System;
using System.Collections.Generic;

namespace WBEADMS
{

    /* TODO:
     *   Consider refactoring the paginator:
     *     1. Create a generic class PagedList<T> that will page through the source list using BaseModel (idea from http://blog.wekeroad.com/2007/12/10/aspnet-mvc-pagedlistt/)
     *     2. Create an HtmlHelper extension to create the prev/next/page links to replace Paginator (similiar to NumericPager in http://blog.wekeroad.com/blog/asp-net-mvc-avoiding-tag-soup/)
     */

    public class Paginator
    {
        public const short DefaultSurroundPages = 2;
        public const int DefaultPageSize = 40;
        private string _url;
        private int _currentPage;
        private int _pageSize;
        private int _firstPage = 1;
        private int _lastPage;
        private int _itemCount;
        private short _surroundPages;

        // default page size with no additional parameters.
        public Paginator(string url, int currentPage, int itemCount, short surroundPages)
            : this(url, currentPage, DefaultPageSize, itemCount, surroundPages) { }

        // default page size, default surround pages.
        public Paginator(string url, int currentPage, int itemCount)
            : this(url, currentPage, DefaultPageSize, itemCount, DefaultSurroundPages) { }

        // default default surround pages.
        public Paginator(string url, int currentPage, int pageSize, int itemCount)
            : this(url, currentPage, pageSize, itemCount, DefaultSurroundPages) { }

        public Paginator(string url, int currentPage, int pageSize, int itemCount, short surroundPages)
        {
            _url = url;
            PageSize = pageSize; // this must be calculated before ItemCount is set to ensure that pageSize is not zero.
            ItemCount = itemCount; // this must be calculated before currentPage is set to ensure that the current page is in the range.
            CurrentPage = currentPage;
            SurroundPages = surroundPages;
        }

        #region Properties
        public int CurrentPage
        {
            get
            {
                if (_currentPage < 1)
                {
                    return 1;
                }

                if (_currentPage > _lastPage)
                {
                    return _lastPage;
                }

                return _currentPage;
            }

            set
            {
                _currentPage = value;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                if (value < 1)
                {
                    _pageSize = 1;

                }
                else
                {
                    _pageSize = value;
                }

                UpdateLastPage();
            }
        }

        public int ItemCount
        {
            get
            {
                return _itemCount;
            }

            set
            {
                _itemCount = value;
                UpdateLastPage();
            }
        }

        public short SurroundPages
        {
            get
            {
                return _surroundPages;
            }

            set
            {
                if (value < 1)
                {
                    _surroundPages = 1;
                    return;
                }

                _surroundPages = value;
            }
        }

        public int LastPage
        {
            get
            {
                return _lastPage;
            }
        }
        #endregion

        private void UpdateLastPage()
        {
            // NOTE: this will fail if itemTotal is 0 will result in page 0.
            // pageSize will always be set by the constructor.
            _lastPage = 1 + ((_itemCount - 1) / _pageSize);
        }

        private int PrevPage
        {
            get { return _currentPage - 1; }
        }

        private int NextPage
        {
            get { return _currentPage + 1; }
        }

        public override string ToString()
        {
            var p = new List<string>();
            int beforePage = _currentPage - _surroundPages;
            int afterPage = _currentPage + _surroundPages;
            int tmpbefore = beforePage;
            int tmpafter = afterPage;


            //if the current page is after the last then make it the last page.
            if (_currentPage > _lastPage)
            {
                _currentPage = _lastPage;
            }

            ////int surroundPages = _surroundPages;

            if (beforePage <= _firstPage)
            {
                tmpafter += _firstPage - beforePage;
                tmpbefore = _firstPage + 1;
            }

            if (afterPage >= _lastPage)
            {
                tmpbefore -= afterPage - _lastPage;
                tmpafter = afterPage = _lastPage - 1;
            }

            beforePage = (tmpbefore > _firstPage) ? tmpbefore : _firstPage + 1;
            afterPage = (tmpafter < _lastPage) ? tmpafter : _lastPage - 1;

            // before current links
            if (_currentPage != _firstPage)
            {
                p.Add(ActionLink(PrevPage, "&lt;&lt;prev", "prevLink")); // "<<PREV"
                p.Add(ActionLink(_firstPage)); // "1"
            }

            if (beforePage - 1 > _firstPage)
            {
                p.Add("...");
            }

            for (int i = beforePage; i < _currentPage; i++)
            {
                p.Add(ActionLink(i)); // "...2 3 4"
            }

            // current page
            p.Add("<span class=\"paginationCurrentPage\">" + _currentPage.ToString() + "</span>");

            // after current links
            for (int i = _currentPage + 1; i <= afterPage; i++)
            {
                p.Add(ActionLink(i)); // "6 7 8..."
            }

            if (afterPage + 1 < _lastPage)
            {
                p.Add("...");
            }

            if (_currentPage != _lastPage)
            {
                p.Add(ActionLink(_lastPage));
                p.Add(ActionLink(NextPage, "next&gt;&gt", "prevLink")); // "NEXT>>"
            }
            string paginationHtml = String.Join(" ", p.ToArray());

            // generate page size form on pagination
            paginationHtml += "<form id=\"pagination\" name=\"pagination\"><select name=\"page_size\"  onchange=\"setUrlParam([{ 'name': 'page_size', 'value': $(this).val() }, { 'name': 'page', 'value': 1 }]);\">";
            /*
            paginationHtml += @"
                <form id=""pagination"" name=""pagination"">
                    <select name=""page_size"" onchange=""setUrlParam([{ 'name': 'page_size', 'value': $('select[name = page_size]').val() }]);"">" + "\n";
            */


            int[] pageSizes = new int[] { 20, 40, 60, 80, 100 };
            foreach (int pageSizeText in pageSizes)
            {
                paginationHtml +=
                    pageSizeText == PageSize
                        ? "<option selected>" + pageSizeText + "</option>"
                        : "<option>" + pageSizeText + "</option>";
            }

            paginationHtml += "</select></form>";

            return paginationHtml;
        }

        public string ActionLink(int pageNumber) { return ActionLink(pageNumber, pageNumber.ToString(), ""); }

        public string ActionLink(int pageNumber, string msg, string cssClass)
        {
            if (_url.Contains("?"))
            {
                return "<a class=\"paginationLink " + cssClass + "\" href=\"" + _url + "&page=" + pageNumber.ToString() + "\"><span>" + msg + "</span></a>";
            }

            return "<a class=\"paginationLink " + cssClass + "\" href=\"" + _url + "?page=" + pageNumber.ToString() + "\"><span>" + msg + "</span></a>";
        }
    }
}