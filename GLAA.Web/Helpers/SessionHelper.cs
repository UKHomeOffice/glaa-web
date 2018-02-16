using GLAA.Web.FormLogic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GLAA.Web.Helpers
{
    public class SessionHelper : ISessionHelper
    {
        public static string LicenceId = "LicenceId";
        public static string CurrentUserIsAdmin = "CurrentUserIsAdmin";

        public HttpContext context { get; set; }

        private ISession session => context.Session;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }

        public string GetString(string key)
        {
            return session.GetString(key);
        }

        public void SetString(string key, string value)
        {
            session.SetString(key, value);
        }

        public void SetInt(string key, int value)
        {
            session.SetInt32(key, value);
        }

        public int GetInt(string key)
        {
            return session.GetInt32(key) ?? 0;
        }

        public void SetBool(string key, bool value)
        {
            session.SetString(key, value.ToString());
        }

        public bool GetBool(string key)
        {
            return bool.Parse(session.GetString(key));
        }

        public void SetSubmittedPage(FormSection section, int id)
        {
            SetString("LastSubmittedPageSection", section.ToString());
            SetInt("LastSubmittedPageId", id);
        }

        public void SetLoadedPage(int id)
        {
            SetInt("LastLoadedPageId", id);
        }

        public int GetLoadedPage()
        {
            return GetInt("LastLoadedPageId");
        }

        public int GetCurrentPaId()
        {
            return GetInt("PaId");
        }

        public bool GetCurrentPaIsDirector()
        {
            return GetBool("IsDirector");
        }

        public void SetCurrentPaStatus(int id, bool isDirector)
        {
            SetInt("PaId", id);
            SetBool("IsDirector", isDirector);
        }

        public void ClearCurrentPaStatus()
        {
            SetCurrentPaStatus(0, false);
        }

        public void SetCurrentLicenceId(int id)
        {
            SetInt(LicenceId, id);
        }

        public int GetCurrentLicenceId()
        {
            return GetInt(LicenceId);
        }

        public int GetCurrentAbrId()
        {
            return GetInt("AbrId");
        }

        public int GetCurrentNamedIndividualId()
        {
            return GetInt("NamedIndividualId");
        }

        public void SetCurrentAbrId(int id)
        {
            SetInt("AbrId", id);
        }

        public void SetCurrentNamedIndividualId(int id)
        {
            SetInt("NamedIndividualId", id);
        }

        public void ClearCurrentAbrId()
        {
            SetCurrentAbrId(0);
        }

        public void ClearCurrentNamedIndividualId()
        {
            SetCurrentNamedIndividualId(0);
        }

        public int GetCurrentDopId()
        {
            return GetInt("DopId");
        }

        public bool GetCurrentDopIsPa()
        {
            return GetBool("IsPa");
        }

        public void SetCurrentDopStatus(int id, bool isPa)
        {
            SetInt("DopId", id);
            SetBool("IsPa", isPa);
        }

        public void ClearCurrentDopStatus()
        {
            SetCurrentDopStatus(0, false);
        }

        // TODO: Fix this when we add proper roles
        public void SetCurrentUserIsAdmin(bool isAdmin)
        {
            SetBool(CurrentUserIsAdmin, isAdmin);
        }

        public bool GetCurrentUserIsAdmin()
        {
            return GetBool(CurrentUserIsAdmin);
        }

        public static int GetInt(ISession session, string key)
        {
            return session.GetInt32(key) ?? 0;
        }

        public static bool GetBool(ISession session, string key)
        {
            var value = false;
            var sessionValue = session.GetString(key);
            if (sessionValue != null)
            {
                bool.TryParse(sessionValue, out value);
            }
            return value;
        }

        public static int? GetCurrentLicenceId(ISession session)
        {
            return session.GetInt32(LicenceId);
        }

        public static bool GetCurrentUserIsAdmin(ISession session)
        {
            return GetBool(session, CurrentUserIsAdmin);
        }

        public T Get<T>(string key) where T : new()
        {
            var objectString = session.GetString(key);

            return session.GetString(key) == null ? new T() : JsonConvert.DeserializeObject<T>(objectString);
        }

        public void Set<T>(string key, T instance)
        {
            var objectString = JsonConvert.SerializeObject(instance);

            SetString(key, objectString);
        }
    }
}