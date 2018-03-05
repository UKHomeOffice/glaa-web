using GLAA.Web.FormLogic;

namespace GLAA.Web.Helpers
{
    public interface ISessionHelper
    {
        string GetString(string key);
        void SetString(string key, string value);
        void SetInt(string key, int value);
        int GetInt(string key);
        bool GetBool(string key);
        [System.Obsolete]
        void SetSubmittedPage(FormSection section, int id);
        void SetSubmittedPage(FormSection section, string actionName);
        [System.Obsolete]
        void SetLoadedPage(int id);
        void SetLoadedPage(string actionName);
        [System.Obsolete]
        int GetLoadedPage();
        string GetLoadedActionName();
        int GetCurrentPaId();
        bool GetCurrentPaIsDirector();
        void SetCurrentPaStatus(int id, bool isDirector);
        void ClearCurrentPaStatus();
        void SetCurrentLicenceId(int id);
        int GetCurrentLicenceId();
        int GetCurrentAbrId();
        int GetCurrentNamedIndividualId();
        void SetCurrentAbrId(int id);
        void SetCurrentNamedIndividualId(int id);
        void ClearCurrentAbrId();
        void ClearCurrentNamedIndividualId();
        int GetCurrentDopId();
        bool GetCurrentDopIsPa();
        void SetCurrentDopStatus(int id, bool isPa);
        void ClearCurrentDopStatus();
        void SetCurrentUserIsAdmin(bool isAdmin);
        bool GetCurrentUserIsAdmin();
        T Get<T>(string key) where T : new();
        void Set<T>(string key, T instance);
    }
}