using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Test1001_shopcar.Models;

namespace Test1001_shopcar.Controllers
{
    public class AccountController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration Post Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "")] AspNetUser AspNetUser)//[Bind(Exclude = "ID")] AspNetUser user
        {
            bool Status = false;
            string message = "";
            // Model Validation 
            if (ModelState.IsValid)
            {
                //#region Generate Activation Code 
                //user.ActivationCode = Guid.NewGuid();
                //#endregion
                
                //#region  Password Hashing 
                //Users.pssseord = Crypto.Hash(user.Password);
                //user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                //#endregion
                //user.IsEmailVerified = false;


                #region Save to Database
                using (CartsEntities db = new CartsEntities())
                {
                    db.AspNetUsers.Add(AspNetUser);
                    
                        db.SaveChanges();
                   
                    
                }
                    //Send Email to User
                    //SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    //message = "Registration successfully done. Account activation link " + 
                    //    " has been sent to your email id:" + user.EmailID;
                    Status = true;
                
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(AspNetUser);


        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Userlogin login)
        {
            string message = "";
            using (CartsEntities db = new CartsEntities())
            {
                var v = db.AspNetUsers.Where(a => a.Username == login.Username).FirstOrDefault();
                if (v != null)
                {
                    //if (!v.IsEmailVerified)
                    //{
                    //    ViewBag.Message = "Please verify your email first";
                    //    return View();
                    //}

                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 1)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.Username, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("Index", "Home");
                    }


                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }



        //LOgOut
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}