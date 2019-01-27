using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    [RequireComponent (typeof (TextMeshProUGUI))]
    public class OpenLinks : MonoBehaviour, IPointerClickHandler
    {
        public string TwitchUrl = "";
        public string TwitterUrl = "";
        public TextMeshProUGUI Label;

        void OnValidate ()
        {
            if (!Label)
            {
                Label = GetComponent<TextMeshProUGUI> ();
            }
        }
        public void OnPointerClick (PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink (Label, Input.mousePosition, eventData.pressEventCamera);
            Debug.Log ("Click index: " + linkIndex);
            if (linkIndex != -1)
            { // was a link clicked?
                TMP_LinkInfo linkInfo = Label.textInfo.linkInfo[linkIndex];

                // open the link id as a url, which is the metadata we added in the text field
                var linkId = linkInfo.GetLinkID ();
                Debug.Log ("Link id: " + linkId + " : "+linkInfo.GetLinkText());
                if (linkId == "twitter")
                {
                    Application.OpenURL (TwitterUrl);
                }
                else
                {
                    if (linkId == "twitch")
                    {
                        Application.OpenURL (TwitchUrl);
                    }
                }

            }
        }
    }
}
