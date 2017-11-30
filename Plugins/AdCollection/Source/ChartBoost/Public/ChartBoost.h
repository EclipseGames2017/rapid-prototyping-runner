/*
* EZ-Mobile-Ads - unreal engine 4 ads plugin
*
* Copyright (C) 2017 feiwu <feixuwu@outlook.com> All Rights Reserved.
*/

#pragma once

#include "ModuleManager.h"
#include "AdCollection.h"

class FChartBoostModule : public IAdModuleInterface
{
public:

	/** IModuleInterface implementation */
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
	virtual void PlayRewardedVideo() override;
	virtual void ShowInterstitialAd() override;
	virtual bool IsInterstitalReady() override;
	virtual bool IsRewardedVideoReady() override;
};