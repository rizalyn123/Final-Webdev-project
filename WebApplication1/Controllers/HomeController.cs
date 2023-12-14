using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetStarted()
        {
            ViewBag.Message = "Your register page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your login page.";

            return View();
        }

        public ActionResult User()
        {
            ViewBag.Message = "Your user page.";

            return View();
        }

        public ActionResult adminDashboard()
        {
            ViewBag.Message = "Your register page.";

            return View();
        }
        public ActionResult Admin()
        {
            accountEntities1 ae = new accountEntities1();

            var userList = (from a in ae.users select a).ToList();
            ViewData["UserList"] = userList;
            return View();
        }

        public ActionResult formtoadd()
        {
            ViewBag.Message = "Your formtoadd pag";

            return RedirectToAction("User");
        }

        public ActionResult updatePage(int id)
        {
            int actID = id;

            accountEntities1 ac = new accountEntities1();

            activity at = (from a in ac.activities
                           where a.activityId == actID
                           select a).FirstOrDefault();

           


            return View();
        }

        public ActionResult Display()
        {
            accountEntities1 ae = new accountEntities1();

            var activityList = (from a in ae.activities select a).ToList();
            ViewData["ActivityList"] = activityList;
            return View();

        }

        public ActionResult InsertData(FormCollection fc)
        {
            String firstname = fc["firstname"];
            String lastname = fc["lastname"];

            String email = fc["email"];
            String password = fc["password"];
            


            user u = new user();

            u.firstname = firstname;
            u.lastname = lastname;
            u.email = email;
            u.password = password;
            u.roleId = 2;

            accountEntities1 ae = new accountEntities1();

            ae.users.Add(u);
            ae.SaveChanges();

            return RedirectToAction("login");
        }

        public ActionResult ReadData(FormCollection fc)
        {
            string email = fc["email"];
            string password = fc["password"];

            accountEntities1 rae = new accountEntities1();

            // Assuming there is a 'users' DbSet in your database context
            user authenticatedUser = rae.users.FirstOrDefault(u => u.password == password && u.email == email);

            // Check if the user was found
            if (authenticatedUser != null)
            {
                if (authenticatedUser.roleId == 2)
                {
                    Session["firstname"] = authenticatedUser.firstname;
                    Session["lastname"] = authenticatedUser.lastname;
                    Session["userId"] = authenticatedUser.userId;
                    //  ViewData["admin"] = authenticatedUser;
                    return RedirectToAction("display");
                }
                else if (authenticatedUser.roleId == 1)
                {
                    Session["firstname"] = authenticatedUser.firstname;
                    Session["lastname"] = authenticatedUser.lastname;
                    Session["userId"] = authenticatedUser.userId;
                    // ViewData["user"] = authenticatedUser;
                    return RedirectToAction("adminDashboard");
                }


            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid credentials. Please try again.";
            }

            // Always return the "Login" view, whether authentication succeeds or fails
            return View("Login");
        }

        public ActionResult addActivity(FormCollection fc)
        {
            String title = fc["title"];
            String date = fc["date"];
            String time = fc["time"];
            String location = fc["location"];
            String ootd = fc["ootd"];
      
            activity a = new activity();

            // Set the userId property of the activity to the currently logged-in user's ID
            a.userId = (int)Session["userId"];
            a.title = title;
            a.date = date;
            a.time = time;
            a.location = location;
            a.ootd = ootd;
       
            accountEntities1 ae = new accountEntities1();

            ae.activities.Add(a);
            ae.SaveChanges();

            return RedirectToAction("display");
        }

        // Add this action in your HomeController or relevant controller
        public ActionResult EditActivity(int id)
        {
            // Fetch the activity from the database based on the provided id
            accountEntities1 ae = new accountEntities1();
            activity existingActivity = ae.activities.Find(id);

            // Check if the activity exists
            if (existingActivity == null)
            {
                return HttpNotFound(); // Or any appropriate action for not found
            }

            // Pass the activity to the view for editing
            return View(existingActivity);
        }

        [HttpPost]
        public ActionResult UpdateActivity(activity updatedActivity)
        {
            if (ModelState.IsValid)
            {
                // Update the existing activity in the database
                accountEntities1 ae = new accountEntities1();
                ae.Entry(updatedActivity).State = EntityState.Modified;
                ae.SaveChanges();

                return RedirectToAction("display");
            }

            // If the model state is not valid, return to the edit view with validation errors
            return View("EditActivity", updatedActivity);
        }

        public ActionResult DeleteActivity(int id)
        {
            accountEntities1 ae = new accountEntities1();

            // Fetch the activity from the database based on the provided id
            activity activityToDelete = ae.activities.Find(id);

            // Check if the activity exists
            if (activityToDelete == null)
            {
                return HttpNotFound(); // Or any appropriate action for not found
            }

            // Remove the activity from the DbSet and save changes
            ae.activities.Remove(activityToDelete);
            ae.SaveChanges();

            return RedirectToAction("display");
        }

        public ActionResult DeleteConfirmation(int id)
        {
            accountEntities1 ae = new accountEntities1();
            activity activityToDelete = ae.activities.Find(id);

            // Check if the activity exists
            if (activityToDelete == null)
            {
                return HttpNotFound(); // Or any appropriate action for not found
            }

            return View(activityToDelete);
        }

        public ActionResult DeleteUser(int id)
        {
            accountEntities1 ae = new accountEntities1();

            // Fetch the activity from the database based on the provided id
            user userToDelete = ae.users.Find(id);

            // Check if the activity exists
            if (userToDelete == null)
            {
                return HttpNotFound(); // Or any appropriate action for not found
            }

            // Remove the activity from the DbSet and save changes
            ae.users.Remove(userToDelete);
            ae.SaveChanges();

            return RedirectToAction("Admin");
        }

        public ActionResult Logout()
        {
            // Clear all session variables
            Session.Clear();

            // Abandon the session
            Session.Abandon();

            // Prevent caching of sensitive pages
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();

            // Redirect to the login page or any other desired page
            return RedirectToAction("Login", "Home");
        }


    }

}