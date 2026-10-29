using System.Data;
using System.Data.SqlClient;
namespace CoreExpenseTracker.Models
{
    public class RegisterDB
    {
        SqlConnection con=new SqlConnection(@"server=INSPIRON\SQLEXPRESS;database=ExpenseTracker;Integrated security=true");
        public string InsertDB(RegisterModel objcls)
        {
            try
            {
                // Save uploaded photo
                string fname = Guid.NewGuid().ToString()
                    + Path.GetExtension(objcls.PhotoFile.FileName);

                string fpath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "photos", fname);

                using (var fs = new FileStream(fpath, FileMode.Create))
                {
                    objcls.PhotoFile.CopyTo(fs);
                }

                // Store the photo path in the model
                objcls.Photo = "/photos/" + fname;

                SqlCommand cmd = new SqlCommand("sp_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", objcls.Name);
                cmd.Parameters.AddWithValue("@email", objcls.Email);
                cmd.Parameters.AddWithValue("@photo", objcls.Photo);
                cmd.Parameters.AddWithValue("@prof",objcls.Profession);
                cmd.Parameters.AddWithValue("@una", objcls.Username);
                cmd.Parameters.AddWithValue("@pwd", objcls.Password);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return "Registered successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                return ex.Message;
            }
        }

        public int LoginDB(LoginModel objcls)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_login", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@una", objcls.Username);
                cmd.Parameters.AddWithValue("@pwd", objcls.Password);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                return count;
            }
            catch
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                throw;
            }
        }

        public int GetUserIdDB(LoginModel objcls)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_getId", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@una", objcls.Username);
                cmd.Parameters.AddWithValue("@pwd", objcls.Password);

                con.Open();
                int uid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                return uid;
            }
            catch
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                throw;
            }
        }

        public string InsertTransactionDB(TransactionModel obj, int userId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@CategoryId", obj.CategoryId);
                cmd.Parameters.AddWithValue("@Amount", obj.Amount);
                cmd.Parameters.AddWithValue("@TransactionType", obj.TransactionType);
                cmd.Parameters.AddWithValue("@TransDate", obj.TransDate);
                cmd.Parameters.AddWithValue(
                    "@Description",
                    string.IsNullOrWhiteSpace(obj.Description)
                        ? DBNull.Value
                        : obj.Description
                );

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return "Transaction added successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                return ex.Message;
            }
        }

        public DataTable GetCategoriesDB(string type, int userId)
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT CategoryId, CategoryName FROM Category_Tab " +
                "WHERE CategoryType = @type " +
                "AND (UserId IS NULL OR UserId = @uid)",
                con);

            da.SelectCommand.Parameters.AddWithValue("@type", type);
            da.SelectCommand.Parameters.AddWithValue("@uid", userId);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public DashboardModel GetDashboardDB(int userId)
        {
            DashboardModel obj = new DashboardModel();

            SqlCommand cmd = new SqlCommand("sp_DashboardSummary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                obj.MonthlyIncome = Convert.ToDecimal(dr["MonthlyIncome"]);
                obj.MonthlyExpenses = Convert.ToDecimal(dr["MonthlyExpenses"]);
                obj.TodayExpenses = Convert.ToDecimal(dr["TodayExpenses"]);
            }

            dr.Close();
            con.Close();

            return obj;
        }

        public DataTable GetMonthlyExpenseCategoryDB(int userId)
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("sp_MonthlyExpenseCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable GetRecentTransactionsDB(int userId)
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("sp_RecentTransactions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public UserProfile GetUserProfileDB(int userId)
        {
            UserProfile obj = new UserProfile();

            SqlCommand cmd = new SqlCommand("sp_GetUserProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                obj.UserId = Convert.ToInt32(dr["UserId"]);
                obj.Name = dr["Name"].ToString();
                obj.Email = dr["Email"].ToString();
                obj.Photo = dr["Photo"].ToString();
                obj.Profession = dr["Profession"].ToString();
                obj.Username = dr["Username"].ToString();
            }

            dr.Close();
            con.Close();

            return obj;
        }

        public string UpdateProfileDB(UserProfile obj)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("dbo.sp_UpdateProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", obj.UserId);
                cmd.Parameters.AddWithValue("@Photo", obj.Photo);
                cmd.Parameters.AddWithValue("@Profession", obj.Profession);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return "Profile updated successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                return ex.Message;
            }
        }

        public MonthlyAnalysisModel GetMonthlyAnalysisDB(
    int userId, int month, int year)
        {
            MonthlyAnalysisModel obj = new MonthlyAnalysisModel();

            obj.Month = new DateTime(year, month, 1).ToString("yyyy-MM");

            SqlCommand cmd = new SqlCommand(
                "sp_MonthlyAnalysisData", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            // First result: monthly income and expenses
            if (dr.Read())
            {
                obj.MonthlyIncome =
                    Convert.ToDecimal(dr["MonthlyIncome"]);

                obj.MonthlyExpenses =
                    Convert.ToDecimal(dr["MonthlyExpenses"]);
            }

            // Second result: category-wise expenses
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    string category = dr["CategoryName"].ToString() ?? "";
                    decimal amount = Convert.ToDecimal(dr["TotalAmount"]);

                    obj.Categories[category] = amount;
                }
            }

            dr.Close();
            con.Close();

            return obj;
        }


    }
}
