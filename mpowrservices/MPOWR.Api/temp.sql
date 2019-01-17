insert into ##temp 
select 
Partner_name,
cast(mot.Previous_Period_MGO_ROI as  decimal(16,1)) as MGO_ROI,
pb.Membership_Type, 
Business_Unit,
Round(Last_year_Sellout,0) as Last_Period_Sellout, 
cast(mot.Sellout_Silver_Below as  decimal(16,0))    as Sellout_Silver_Below,
cast(mot.Sellout_Gold_Platinum as  decimal(16,0))   as Sellout_Gold_Platinum,
cast(((mot.Sellout_Silver_Below) + ( mot.Sellout_Gold_Platinum)) as  decimal(16,0)) as Unweighted_Sellout,

  case when mot.Prev_To_Prv_Year_sellout is null or mot.Prev_To_Prv_Year_sellout<> 0
		   then 
	       cast( ((mot.Last_year_Sellout - mot.Prev_To_Prv_Year_sellout)/mot.Prev_To_Prv_Year_sellout) as  decimal(16,1)) *100 
		   else cast(0 as  decimal(16,1)) end as  YoY_Sellout,

cast(mot.Scaled_Sellout as  decimal(16,1))  as Scaled_Sellout,

case when mot.Last_year_Sellout is null or mot.Last_year_Sellout<> 0
then 
cast( ((mot.Last_Period_Sellout - mot.Last_year_Sellout)/mot.Last_year_Sellout) as  decimal(16,1)) *100 
else cast(0 as  decimal(16,1)) end as  HOH_SellOut,

Round(projected_sellout,0) as projected_sellout,
 
Source_of_SellOut as Source_of_sellout ,

case when mot.Last_year_Sellout is null or mot.Last_year_Sellout<>0
		    then
		    cast( ((mot.[Projected_Sellout]  - mot.Last_year_Sellout)/mot.Last_year_Sellout) as  decimal(16,1)) *100 
		    else cast(0 as  decimal(16,1)) end as      YOY_Currenty_PreviousY,

ROUND(isnull(recommended_mdf,0),0) as RecommendedMDF, 
ROUND(isnull(additional_recommendedmdf,0),0) as Additional_RecommendedMDF, 
isnull(PrevPrevPeriodMDF,0) as Alloc_PrevY_MDF, 
cast(mot.Last_year_mdf as  decimal(16,1)) as Last_year_mdf,
ROUND(isnull(pub.MSA,0),0)  as msa, --cast(0 as  decimal(16,1))

case when  ( (CHARINDEX('26',@CountryID) > 0  or @allocationLevel = 'D' )and  @PartnerTypeID = 2 and Business_Unit = 'Aruba') -- checking for country usa and canada , reseler parner type
then   ISNULL(cast(pb.aruba_msa as  decimal(16,1)), 0 )
else cast(0 as  decimal(16,1))  end as arubamsa,
isnull(PrevPeriodMDF,0) as Alloc_PrevP_MDF, 
Round(Last_Period_MDF,0) as Last_Period_MDF ,
ROUND(isnull(TotalMDF,0),0) as AllocatedMDFR1, 
ROUND(isnull(pub.Baseline_MDF,0),0) as Total_CurY_MDF,
ROUND(isnull(CarveOut,0),0) as Alloc_Carv_MDF,
ROUND( isnull(additional_mdf,0),0) as AllocatedMDFR2 ,
cast( ( ( ISNULL((pub.Additional_MDF ), 0 )+pub.Baseline_MDF) - mot.Last_Period_MDF ) as  decimal(16,1) ) as MDF_CurrentY_PreviousY_Delta,
Cast(YoY_change_MDF*100 as  decimal(16,1)) as YoY_change_MDF, 

cast((case when  (mot.Last_Period_MDF) >0 then ( ((ISNULL( (pub.Additional_MDF ), 0 )+pub.Baseline_MDF)- (mot.Last_Period_MDF))/  (mot.Last_Period_MDF)) else 0 end)* 100 as  decimal(16,1))   as HOH_CurrentP_previousP,

Cast(Last_Period_Productivity*100 as  decimal(16,1)) as Last_Period_Productivity,

case when mot.Last_Period_Sellout is null or mot.Last_Period_Sellout<>0
then
cast(((mot.Last_Period_MDF)/(mot.Last_Period_Sellout))*100 as  decimal(16,1))  
else cast(0 as  decimal(16,1)) end as      previosp_MDF_vs_Sellpout,

case when isnull(projected_sellout,0) = 0 then 0
else  Round(((isnull(baseline_mdf,0) + isnull(additional_mdf,0))/projected_sellout)*100,1) end as Projected_Productivity,

cast(mot.Median_Avg_MDF_Sellout*100  as  decimal(16,1)) as Median_Avg_MDF_Sellout,

case when  (mot.Last_year_Sellout) =0 OR  (Projected_Sellout) =0 
then 0
when  ((mot.Last_year_mdf)/ (mot.Last_year_Sellout) )= 0
then 0

when ( (isnull(baseline_mdf,0) + isnull(additional_mdf,0))/ (Projected_Sellout))  =0 
then 0
when isnull(( (mot.Last_year_mdf)/ (mot.Last_year_Sellout)),0) = 0 and isnull((  (isnull(baseline_mdf,0) + isnull(additional_mdf,0))/ (Projected_Sellout)),0) >0 
then 100 
else (1-(  (isnull(baseline_mdf,0) + isnull(additional_mdf,0))/ (Projected_Sellout))/( (mot.Last_year_mdf)/ (mot.Last_year_Sellout)))*100  end as ProductivityImprovement,

cast(mot.SOW as  decimal(16,1)) as SOW, 
ISNULL(cast(mot.WMGO_Ratio as  decimal(16,1)), 0 )  as  WMGORatio,
cast(mot.Previous_Period_MGO as  decimal(16,1)) as MGO,
cast(mot.Previous_Period_Won_MGO as  decimal(16,1)) as W_MGO,
cast(mot.Previous_Period_Won_MGO_ROI as  decimal(16,1)) as W_MGO_ROI,
mot.PartnerID,
Geo.DisplayName as GEO,
con.Displayname as Country,
dis.DisplayName as District,
pt.DisplayName as Budget, 
Prediction_Accuracy, 
cast ( mot.ProjectionMethod as varchar(15)) as ProjectionMethod,
PREV_MDF_Assessment,
MDF_Alignment,  
Variance.Reason
 