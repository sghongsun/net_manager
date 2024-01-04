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
using System.Transactions;
using System.Diagnostics.Metrics;

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

        public void insertTerms(TermsAddRequest termsAddRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(termsQuery.insert_terms(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@title", termsAddRequest.title));
                        cmd.Parameters.Add(new MySqlParameter("@place", termsAddRequest.place));
                        cmd.Parameters.Add(new MySqlParameter("@contents", termsAddRequest.contents));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public TermsModel getTermsInfo(int idx)
        {
            TermsModel model = new TermsModel();

            using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
            {
                dbcon.Open();

                using (var cmd = new MySqlCommand(termsQuery.select_terms_by_idx(), dbcon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new MySqlParameter("@idx", idx));

                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        model.idx = Convert.ToInt32(dr["idx"]);
                        model.title = dr["title"].ToString();
                        model.place = dr["place"].ToString();
                        model.contents = dr["contents"].ToString();
                        model.createid = dr["createid"].ToString();
                        model.createdt = Convert.ToDateTime(dr["createdt"]);
                    }
                    dr.Close();
                }                

                dbcon.Close();
                dbcon.Dispose();
            }

            return model;
        }

        public void updateTmers(TermsModifyRequest termsModifyRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(termsQuery.update_terms(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@idx", termsModifyRequest.idx));
                        cmd.Parameters.Add(new MySqlParameter("@title", termsModifyRequest.title));
                        cmd.Parameters.Add(new MySqlParameter("@place", termsModifyRequest.place));
                        cmd.Parameters.Add(new MySqlParameter("@contents", termsModifyRequest.contents));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }

        public void deleteTmers(TermsDeleteRequest termsDeleteRequest)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (MySqlConnection dbcon = new MySqlConnection(SetInfo.m_dbconn))
                {
                    dbcon.Open();

                    using (var cmd = new MySqlCommand(termsQuery.update_terms_for_delflag(), dbcon))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new MySqlParameter("@idx", termsDeleteRequest.idx));
                        cmd.Parameters.Add(new MySqlParameter("@id", Func.GetCookie("adminid")));
                        cmd.Parameters.Add(new MySqlParameter("@ip", Func.GetCookie("ip")));

                        cmd.ExecuteNonQuery();
                    }

                    ts.Complete();

                    dbcon.Close();
                    dbcon.Dispose();
                }
            }
        }
    }
}