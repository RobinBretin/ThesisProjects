#define USE_VS_ATTRIBUTION

using System;
using UnityEngine.Analytics;

namespace Soundxr
{
	public static class VSAttributionWrapper
	{
		const string k_PartnerName = "Yamaha Corporation";
		const string k_CustomerUid = "A97AFAFF-8A56-40DB-AEAF-42C258EFC7FF";

		public static bool SendAttributionEvent(string actionName)
		{
#if USE_VS_ATTRIBUTION
			AnalyticsResult result = VSAttribution.SendAttributionEvent(actionName, k_PartnerName, k_CustomerUid);
			return (result == AnalyticsResult.Ok);
#else
			return false;
#endif
		}
	}
}
