using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace WebApplication4
{
    /// <summary>
    /// Summary description for JokeService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class JokeService : System.Web.Services.WebService
    {

        [WebMethod]
        public Joke GetJokeById(int id)
        {
            Joke joke = new Joke();

            string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetJokeById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@id";
                parameter.Value = id;

                cmd.Parameters.Add(parameter);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    joke.Id = Convert.ToInt32(rdr["id"]);
                    joke.firstSentence = rdr["first_sentance"].ToString();
                    joke.secondSentence = rdr["second_sentance"].ToString();
                }
            }

            return joke;
        }

        [WebMethod]
        public void GetAllJokes()
        {
            List<Joke> jokes = new List<Joke>();

            string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM jokebot", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Joke joke = new Joke();
                    joke.Id = Convert.ToInt32(rdr["id"]);
                    joke.firstSentence = rdr["first_sentance"].ToString();
                    joke.secondSentence = rdr["second_sentance"].ToString();

                    jokes.Add(joke);
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(jokes));

            //return jokes;
        }

        [WebMethod]
        public void AddJoke(Joke newJoke)
        {
            Joke joke = new Joke();

            string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetJokeById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@firstSentence",
                    Value = newJoke.firstSentence
                });

                cmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@secondSentence",
                    Value = newJoke.secondSentence
                });

                con.Open();
                cmd.ExecuteNonQuery();
            }

        }
    }
}
