﻿
var flag=false;var servicePath="/servicepage.aspx";var countryid;var fromFaci=0;var toFaci=6;var flag2=true;var fromMain=0;var toMain=6;var fromFix=0;var toFix=6;var facilityData=[];var unitviewData=[];var fixtureData=[];var developData=[];var currncyData;var financeData;var val;jQuery(document).ready(function(){countryid=jQuery('.buy').attr('countryid')==undefined?35:jQuery('.buy').attr('countryid');jQuery('#adv-srch-drop').on('click',function(){switch(flag){case false:if(flag2){AjaxCallGetAdvaceSearch();AjaxCallOfFacility();AjaxCallOfFixture();}
flag2=false;jQuery('#advancsearchMinusOrPlus').html('(-)');jQuery('#adv_Arrow').removeClass('advance-arrow-top');jQuery('#adv_Arrow').addClass('advance-arrow-bottom');jQuery('#hiddenRefineDiv').show();flag=true;break;case true:jQuery('#advancsearchMinusOrPlus').html('(+)');jQuery('#adv_Arrow').removeClass('advance-arrow-bottom');jQuery('#adv_Arrow').addClass('advance-arrow-top');jQuery('#hiddenRefineDiv').hide();flag=false;break;}});jQuery('.moreviewcls').on('click',function(){AjaxCallGetAdvaceSearch();});jQuery('#facilityColaps').on('click',function(){AjaxCallOfFacility();});jQuery('#fixtureColaps').on('click',function(){AjaxCallOfFixture();});});function AjaxCallGetAdvaceSearch(){var html="";jQuery.ajax({type:"POST",url:servicePath+"/getmainviews",contentType:"application/json; charset=utf-8",data:JSON.stringify({CountryId:countryid,from:fromMain,to:toMain}),datatype:"json",processData:true,success:function(result){if(result.d.length>0){for(var i=0;i<result.d.length;i++){var ViewName=result.d[i].View_name
var ViewPk=result.d[i].View_pk;html+="<div class='dropAdvanceCheckbox'><input id='"+ViewPk+"' type='checkbox' onchange=AdvSearchHiddenFill(this); class='unitviewCheck' value='"+ViewPk+"' ><label for='"+ViewPk+"'>"+ViewName+"</label></div>";}
jQuery('#mainViewDiv').append(html);fromMain+=5;toMain+=5;}},error:function(){alert("ERROR 1")},failure:function(){alert("ERROR Failure");},complete:function(){}});}
function AjaxCallOfFacility(){var html="";jQuery.ajax({type:"POST",url:servicePath+"/getfaci",contentType:"application/json; charset=utf-8",data:JSON.stringify({CountryId:countryid,from:fromFaci,to:toFaci}),datatype:"json",processData:true,success:function(result){if(result.d.length>0){for(var i=0;i<result.d.length;i++){var faciName=result.d[i].Facility_name;var faciPk=result.d[i].Facility_pk;html+="<div class='dropAdvanceCheckbox'><input id='"+faciPk+"' type='checkbox' class='facilityCheck' onchange=AdvSearchHiddenFill(this); name='facilityCheck' value='"+faciPk+"'><label for='"+faciPk+"'>"+faciName+"</label></div>";}
jQuery('#facilityDiv').append(html);fromFaci+=5;toFaci+=5;}},error:function(){alert("ERROR")},failure:function(){alert("ERROR Failure");},complete:function(){}});}
function AjaxCallOfFixture(){var html="";jQuery.ajax({type:"POST",url:servicePath+"/getfixtr",contentType:"application/json; charset=utf-8",data:JSON.stringify({CountryId:countryid,from:fromFix,to:toFix}),datatype:"json",processData:true,success:function(result){if(result.d.length>0){for(var i=0;i<result.d.length;i++){var fixName=result.d[i].Fixtures_name;var fixiPk=result.d[i].Fixtures_pk;html+="<div class='dropAdvanceCheckbox'><input id='"+fixiPk+"' type='checkbox' name='fixtureCheck' class='fixtureCheck' onchange=AdvSearchHiddenFill(this);  value='"+fixiPk+"'><label for='"+fixiPk+"'>"+fixName+"</label></div>";}
jQuery('#fixturesDiv').append(html);fromFix+=5;toFix+=5;}},error:function(){alert("ERROR of AjaxCallOfFixture")},failure:function(){alert("ERROR Failure of AjaxCallOfFixture");},complete:function(){}});}
function AjaxCallForCurrency(){var html="";var counter=0;jQuery.ajax({type:"POST",url:servicePath+"/getcountries",contentType:"application/json;charset=utf-8",data:JSON.stringify({CountryId:countryid}),dataType:"json",processData:true,success:function(result){if(result.d.length>0){var interconversion=result.d[0].CurrencyInterconverstion;var intercArr=new Array();intercArr=interconversion.split(',');var currencyUnit;for(var i=0;i<intercArr.length;i++){var singleCurr=intercArr[i];currencyUnit=singleCurr.substring(singleCurr.lastIndexOf('|'),0);if(currencyUnit==result.d[0].UnitCurrency){var currentCurr=currencyUnit;var select=document.getElementById('ddlcurrncy');var option=document.createElement("option");option.value=currentCurr;option.innerHTML=currentCurr;}
else{var otherCountryCurrency=currencyUnit;var select=document.getElementById('ddlcurrncy');var option=document.createElement("option");option.value=otherCountryCurrency;option.innerHTML=otherCountryCurrency;}
jQuery(select).data("selectBox-selectBoxIt").add(option);}}
else{}},Error:function(){alert("Error");}});}
function AdvSearchHiddenFill(obj){var cls=obj.className
switch(cls){case'facilityCheck':if(obj.checked==true){val=obj.value;facilityData.push(val);updatedata(obj,facilityData);}
else{val=obj.value;RemoveArr(facilityData,val);updatedata(obj,facilityData);}
break;case'unitviewCheck':if(obj.checked==true){val=obj.value;unitviewData.push(val);updatedata(obj,unitviewData);}
else{val=obj.value;RemoveArr(unitviewData,val);updatedata(obj,unitviewData);}
break;case'fixtureCheck':if(obj.checked==true){val=obj.value;fixtureData.push(val);updatedata(obj,fixtureData);}
else{val=obj.value;RemoveArr(fixtureData,val);updatedata(obj,fixtureData);}
break;case'finance':if(obj.checked==true&&obj.value!=null){val=obj.value;financeData=val;updatedata(obj,financeData);}
else{val=obj.value;}
break;case'displayCurr':if(obj.value!=null){val=obj.value;currncyData=val;updatedata(obj,currncyData);}
else{val=obj.value;}
break;case'development':if(obj.checked==true){val=obj.value;developData.push(val);updatedata(obj,developData);}
else{val=obj.value;RemoveArr(developData,val);updatedata(obj,developData);}
break;default:break;}}
function updatedata(obj,data){var cls=obj.className
switch(cls){case'facilityCheck':jQuery('#hdnFaci').val(data.toString());break;case'unitviewCheck':jQuery('#hdnMainUView').val(data.toString());break;case'fixtureCheck':jQuery('#hdnFix').val(data.toString());break;case'finance':jQuery('#hdnFinance').val(data.toString());break;case'displayCurr':jQuery('#hdncurrency').val(data);break;case'development':jQuery('#hdnDevelopment').val(data.toString());break;}}
function RemoveArr(array,value){for(var singleVal in array){if(array[singleVal]==value){array.splice(singleVal,1);break;}}}