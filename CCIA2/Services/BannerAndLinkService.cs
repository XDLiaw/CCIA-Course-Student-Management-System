using CCIA2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CCIA2.Services
{
    public class BannerAndLinkService
    {
        private CCIAContext db;

        public BannerAndLinkService(CCIAContext db)
        {
            this.db = db;
        }

        public void create(BannerAndLink model)
        {
            if (model.uploadFile != null && model.uploadFile.ContentLength > 0)
            {
                model.storeFile();
                model.dbFile.createDate = DateTime.Now;
                db.dbFile.Add(model.dbFile);
                db.SaveChanges();
                model.dbFileSqno = model.dbFile.sqno;
            }

            model.createDate = DateTime.Now;
            db.bannerAndLink.Add(model);
            db.SaveChanges();
        }

        public void edit(BannerAndLink model)
        {
            if (model.uploadFile != null && model.uploadFile.ContentLength > 0)
            {
                model.dbFile = db.dbFile.Where(x => x.sqno == model.dbFileSqno).FirstOrDefault();
                if (model.dbFile == null)
                {
                    model.storeFile();
                    model.dbFile.createDate = DateTime.Now;
                    db.dbFile.Add(model.dbFile);
                    db.SaveChanges();
                    model.dbFileSqno = model.dbFile.sqno;
                }
                else
                {
                    model.storeFile();
                    db.Entry(model.dbFile).State = EntityState.Modified;
                    db.SaveChanges();
                }               
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}