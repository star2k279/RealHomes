var temp;var tempValue;var tempLeft;var scrollerW=0;function bhomesPaging(totalPages,sp_CurrentPage)
{var scrollPane=jQuery('.scroll-pane');var scrollContent=jQuery('.scroll-content');if(parseInt(totalPages)>7)
{var counter=0;jQuery(".scroll-content > div").each(function(){var object=jQuery(this);{scrollerW+=object.width();}
counter++;});jQuery(".scroll-content").width(scrollerW);if(scrollerW<198)
{jQuery(".scroll-pane").width(scrollerW);jQuery(".scroll-bar-wrap").css('display','none');jQuery(".dot").css('display','none');}}
else
{jQuery(".scroll-bar-wrap").css('display','none');jQuery(".dot").css('display','none');}
var scrollbar=jQuery(".scroll-bar").slider({min:1,max:parseInt(totalPages),value:parseInt(sp_CurrentPage),slide:function(e,ui){if(scrollContent.width()>scrollPane.width()){scrollContent.css('margin-left',Math.round(((ui.value)/parseInt(totalPages))*(scrollPane.width()-scrollContent.width()))+'px');if(scrollContent.css('margin-left')==temp)
{scrollContent.css('margin-left',"0px");}}
else{scrollContent.css('margin-left',0);}}});if(scrollContent.width()>scrollPane.width()){temp=Math.round((1/parseInt(totalPages))*(scrollPane.width()-scrollContent.width()))+'px';if(parseInt(sp_CurrentPage)==1)
{tempValue=Math.round((parseInt(sp_CurrentPage)/parseInt(totalPages))*(scrollPane.width()-scrollContent.width()))+34+'px';}
else if((parseInt(totalPages)-parseInt(sp_CurrentPage))<=3)
{tempValue=Math.round((parseInt(sp_CurrentPage)/parseInt(totalPages))*(scrollPane.width()-scrollContent.width()))+'px';}
else
{tempValue=Math.round((parseInt(sp_CurrentPage)/parseInt(totalPages))*(scrollPane.width()-scrollContent.width()))+20+'px';}}
else{temp=0;}
if(scrollContent.width()>scrollPane.width())
{if(parseInt(sp_CurrentPage)<=7)
{jQuery(".scroll-content").css('margin-left','0px');}
else
{jQuery(".scroll-content").css('margin-left',tempValue);}}
tempLeft=jQuery(".ui-slider-handle").css("left");jQuery(".dot").css('left',tempLeft=='100%'?'97%':tempLeft);jQuery(".dot").click(function(){jQuery(".scroll-content").css('margin-left',tempValue);jQuery(".ui-slider-handle").css('left',tempLeft);});var handleHelper=scrollbar.find('.ui-slider-handle').mousedown(function(){scrollbar.width(handleHelper.width());}).mouseup(function(){scrollbar.width('100%');}).append('<span class="ui-icon ui-icon-grip-dotted-vertical"></span>').wrap('<div class="ui-handle-helper-parent"></div>').parent();scrollPane.css('overflow','hidden');function sizeScrollbar(){var remainder=scrollContent.width()-scrollPane.width();var proportion=remainder/scrollContent.width();var handleSize=scrollPane.width()-(proportion*scrollPane.width());scrollbar.find('.ui-slider-handle').css({width:handleSize,'margin-left':-handleSize/2});handleHelper.width('').width(scrollbar.width()-handleSize);}
function resetValue(){var remainder=scrollPane.width()-scrollContent.width();var leftVal=scrollContent.css('margin-left')=='auto'?0:parseInt(scrollContent.css('margin-left'));var percentage=Math.round(leftVal/remainder*100);scrollbar.slider("value",percentage);}
function reflowContent(){var showing=scrollContent.width()+parseInt(scrollContent.css('margin-left'));var gap=scrollPane.width()-showing;if(gap>0){scrollContent.css('margin-left',parseInt(scrollContent.css('margin-left'))+gap);}}
jQuery(window).resize(function(){resetValue();sizeScrollbar();reflowContent();});setTimeout(sizeScrollbar,10);}