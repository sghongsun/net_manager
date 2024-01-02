using Manager.Query.Manage;
using Manager.Models.Manage;
using Manager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manager.Request.Manage;
using System.Security.Cryptography.X509Certificates;
using Manager.Request;
using System.Web.WebPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace Manager.Service.Manage
{
    public class TermsService
    {
        private TermsQuery termsQuery;

        public TermsService()
        {
            this.termsQuery = new TermsQuery();
        }

        public TermsListModel getList(TermsSearch termsSearch)
        {
            string where = "";
            List<SearchParam> searchParam = new List<SearchParam>();
            if (!termsSearch.keyword.IsEmpty())
            {
                if (termsSearch.searchtype.Equals("title"))
                {
                    where += "and title LIKE CONCAT('%', @keyword, '%') ";
                    searchParam.Add(new SearchParam() { name = "@keyword", value = termsSearch.keyword });
                }
                else if (termsSearch.searchtype.Equals("contents"))
                {
                    where += "and contents LIKE CONCAT('%', @keyword, '%') ";
                    searchParam.Add(new SearchParam() { name = "@keyword", value = termsSearch.keyword });
                }
            }

            int count = 0;
            List<TermsModel> termsList = new List<TermsModel>();
            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                string query = "";
                query = termsQuery.select_terms_by_list_for_totalcount().Replace("#searchWhere", where);

                using (var cmd = new MySqlCommand(query, dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    foreach (var param in searchParam)
                    {
                        cmd.Parameters.Add(new MySqlParameter(param.name, param.value));
                    }

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        count = Convert.ToInt32(dr["totalcount"]);
                    }

                    dr.Close();
                }

                if (count < 0)
                {
                    dbcon.Close();
                    dbcon.Dispose();

                    return null;
                }

                termsSearch.pagination = new Pagination(count, termsSearch);

                query = termsQuery.select_terms_by_list().Replace("#searchWhere", where);

                using (var cmd = new MySqlCommand(query, dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    foreach (var param in searchParam)
                    {
                        cmd.Parameters.Add(new MySqlParameter(param.name, param.value));
                    }
                    cmd.Parameters.Add(new MySqlParameter("@startlimit", termsSearch.pagination.limitStart));
                    cmd.Parameters.Add(new MySqlParameter("@endlimit", termsSearch.recordsize));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        TermsModel termsModel = new TermsModel();
                        termsModel.idx = Convert.ToInt32(dr["idx"]);
                        termsModel.title = dr["title"].ToString();
                        termsModel.place = dr["place"].ToString();
                        termsModel.createid = dr["createid"].ToString();
                        termsModel.createdt = Convert.ToDateTime(dr["createdt"]);
                        termsList.Add(termsModel);
                    }

                    dr.Close();
                }

                dbcon.Close();
                dbcon.Dispose();
            }

            TermsListModel model = new TermsListModel()
            {
                Search = termsSearch,
                TermsList = termsList
            };

            return model;
        }
    }
}