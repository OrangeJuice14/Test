using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class MenuRoleApiController : ApiController
    {
        //WebMenu
        [Authorize]
        [Route("")]
        public WebMenuDTO GetObject(Guid id)
        {
            WebMenuDTO result = new WebMenuDTO();
            SessionManager.DoWork(session =>
            {
                WebMenu webmenu = session.Query<WebMenu>().Where(w => w.Id == id).SingleOrDefault();
                result.Id = webmenu.Id;
                result.Name = webmenu.Name;
                result.Index = webmenu.Index;
                result.ParentId = webmenu.ParentMenu != null ? webmenu.ParentMenu.Id : Guid.Empty;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<WebMenuDTO> GetWebMenuTreeList()
        {
            List<WebMenuDTO> result = new List<WebMenuDTO>();
            SessionManager.DoWork(session =>
            {
                Guid root = Guid.Empty;
                List<WebMenu> parentList = new List<WebMenu>();
                List<WebMenu> childList = new List<WebMenu>();
                parentList = session.Query<WebMenu>().Where(w => w.ParentMenu.Id == root).OrderBy(w=>w.Index).ToList();
                foreach (WebMenu pl in parentList)
                {
                    WebMenuDTO pd = pl.Map<WebMenuDTO>();
                    pd.ParentId = null;
                    result.Add(pd);
                }
                childList = session.Query<WebMenu>().Where(w => w.ParentMenu.Id != root && w.ParentMenu.Id != null).OrderBy(w => w.Index).ToList();
                foreach (WebMenu cl in childList)
                {
                    WebMenu temp = session.Query<WebMenu>().Where(w => w.Id == cl.ParentMenu.Id).SingleOrDefault();
                    if (temp.ParentMenu.Id == root)
                    {
                        WebMenuDTO cd = cl.Map<WebMenuDTO>();
                        cd.ParentId = cl.ParentMenu.Id;
                        result.Add(cd);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<WebMenuDTO> GetListParentMenu()
        {
            List<WebMenuDTO> result = new List<WebMenuDTO>();
            SessionManager.DoWork(session =>
            {
                Guid root = Guid.Empty;
                List<WebMenu> parentList = new List<WebMenu>();
                parentList = session.Query<WebMenu>().Where(w => w.ParentMenu.Id == root).OrderBy(w => w.Index).ToList();
                foreach (WebMenu pl in parentList)
                {
                    WebMenuDTO pd = pl.Map<WebMenuDTO>();
                    pd.ParentId = null;
                    result.Add(pd);
                }              
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(WebMenuDTO obj)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                WebMenu web = session.Query<WebMenu>().Where(w => w.Id == obj.Id).SingleOrDefault();
                web.Name = obj.Name;
                web.Index = obj.Index;
                if (web.ParentMenu.Id!=obj.ParentId)
                {
                    WebMenu parent = session.Query<WebMenu>().Where(w => w.Id == obj.ParentId).SingleOrDefault();
                    web.ParentMenu = parent;
                }
                session.SaveOrUpdate(web);
                result = 1;
            });
            return result;
        }

        //WebGroup
        [Authorize]
        [Route("")]
        public WebGroupDTO GetWebGroup(Guid id)
        {
            WebGroupDTO result = new WebGroupDTO();
            SessionManager.DoWork(session =>
            {
                WebGroup web = session.Query<WebGroup>().Where(w => w.Id == id).SingleOrDefault();
                result.Id = web.Id;
                result.Name = web.Name;
                result.WebMenuIds = new List<Guid>();
                foreach (WebMenu w in web.WebMenus)
                {
                    result.WebMenuIds.Add(w.Id);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<WebMenuHierarchyDTO> GetWebMenuHierarchy(Guid webGroupId)
        {
            List<WebMenuHierarchyDTO> result = new List<WebMenuHierarchyDTO>();
            SessionManager.DoWork(session =>
            {
                List<Guid> webMenuIds = new List<Guid>();
                WebGroup g = session.Query<WebGroup>().SingleOrDefault(p => p.Id == webGroupId);
                if (g != null)
                    webMenuIds = g.WebMenus.Select(d => d.Id).ToList();

                Guid root = Guid.Empty;
                List<WebMenu> parentList = new List<WebMenu>();
                parentList = session.Query<WebMenu>().Where(w => w.ParentMenu.Id == root).OrderBy(w => w.Index).ToList();
                foreach (WebMenu p in parentList)
                {
                    WebMenuHierarchyDTO pd = new WebMenuHierarchyDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    result.Add(pd);
                }
                List<WebMenu> childList = new List<WebMenu>();
                List<WebMenuDTO> childListDTO = new List<WebMenuDTO>();
                childList = session.Query<WebMenu>().Where(w => w.ParentMenu.Id != root).OrderBy(w => w.Index).ToList();
                foreach (WebMenu cl in childList)
                {
                    WebMenu temp = session.Query<WebMenu>().Where(w => w.Id == cl.ParentMenu.Id).SingleOrDefault();
                    if (temp.ParentMenu.Id == root)
                    {
                        WebMenuDTO cd = new WebMenuDTO();
                        cd.Id = cl.Id;
                        cd.Name = cl.Name;
                        cd.ParentId = cl.ParentMenu.Id;
                        childListDTO.Add(cd);
                    }
                }
                foreach (WebMenuHierarchyDTO dh in result)
                {
                    dh.items = new List<WebMenuDTO>();
                     foreach (WebMenuDTO dt in childListDTO)
                    {
                        if (dt.ParentId == dh.Id)
                        {
                            if (webMenuIds.Contains(dt.Id))
                                dt.@checked = true;
                            dh.items.Add(dt);
                        }
                    }
                    if (webMenuIds.Contains(dh.Id) && (dh.items.Count<1))
                    {
                        dh.@checked = true;
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<WebGroupDTO> GetListWebGroup()
        {
            List<WebGroupDTO> result = new List<WebGroupDTO>();
            SessionManager.DoWork(session =>
            {
                List<WebGroup> list = new List<WebGroup>();
                list = session.Query<WebGroup>().ToList();
                foreach (WebGroup pl in list)
                {
                    WebGroupDTO pd = pl.Map<WebGroupDTO>();
                    result.Add(pd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public List<Guid> ChildrenMenuIds(Guid id)
        {
            List<Guid> result = new List<Guid>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<WebMenu>().Where(w => w.ParentMenu.Id == id).Select(w=>w.Id).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int PutWebGroup(WebGroupDTO obj)
        {
            int result = 0;
            SessionManager.DoWork(session =>
            {
                WebGroup webGroup = new WebGroup();
                webGroup.Id = obj.Id;
                webGroup.Name = obj.Name;
                webGroup.WebMenus = new List<WebMenu>();
                List<Guid> temp = new List<Guid>();
                //Check menu con lấy menu cha
                foreach (Guid t in obj.WebMenuIds)
                {
                    WebMenu menu = session.Query<WebMenu>().Where(p => p.Id == t).SingleOrDefault();
                    temp.Add(menu.Id);
                    if (menu.ParentMenu != null)
                    {
                        temp.Add(menu.ParentMenu.Id);
                    }
                }
                obj.WebMenuIds = temp.Distinct().ToList();
                foreach (Guid w in obj.WebMenuIds)
                {
                    //Add menu con của hồ sơ, quá trình, đoàn thể
                    if ((w.ToString()== "00000000-0000-0000-0000-000000000017") || (w.ToString() == "00000000-0000-0000-0000-000000000018") || (w.ToString() == "00000000-0000-0000-0000-000000000020"))
                    {
                        WebMenu web = new WebMenu() { Id = w };
                        webGroup.WebMenus.Add(web);
                        List<Guid> childList = ChildrenMenuIds(w);
                        foreach (Guid c in childList)
                        {
                            WebMenu cw = new WebMenu() { Id = c };
                            var exists = webGroup.WebMenus.Where(q => q.Id == cw.Id).FirstOrDefault();
                            if (exists == null)
                                webGroup.WebMenus.Add(cw);
                        }
                    }
                    else
                    {
                        WebMenu web = new WebMenu() { Id = w };
                        var exists = webGroup.WebMenus.Where(q => q.Id == web.Id).FirstOrDefault();
                        if (exists == null)
                            webGroup.WebMenus.Add(web);
                    }
                }
                session.Update(webGroup);
                result = 1;
            });
            return result;
        }
    }
}
