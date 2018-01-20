/*
common.js
函数公共库
*/
var Common = {};

//设置本地缓存

Common = {
	setStorage:function(n,v){
		if(typeof n == 'undefined')return;
		if(window.localStorage){
			localStorage.setItem(n,v);
		}
	},
	getStorage:function(n){
		var result = '';
		if(typeof n == 'undefined')return;
		if(window.localStorage){
			result = localStorage.getItem(n);
		}
		return result;
	},
	clearStorage:function(){
		
	},
	showLoading:function(msg){
		try{
			layer.msg(msg, {
			  icon: 16
			  ,shade: 0.55,
			  time:750
			});
		}catch(e){
			console.warn(e);
		}
	},
	hideLoading:function(times){
		setTimeout(function(){
			try{
				layer.closeAll('loading');
			}catch(e){
				console.warn(e);
			}
		},times);
	},
	alert_me:function(msg){
		try{
			layer.alert(msg, {
			  skin: 'layui-layer-molv' //样式类名
			  ,closeBtn: 0
			}, function(){
			  
			});
		}catch(e){
			console.warn(e);
		}
	},
	tips:function(msg){
		try{
			layer.open({
				content:msg,
				skin:'msg',
				times:2
			});
		}catch(e){
			console.warn(e);
		}
	},
	initAdminAcoout:function(){
		this.showLoading("拼命加载中...");
		$("#loginAccout").text("");
		var userInfo = JSON.parse(this.getStorage("user_info"));
		//console.log(userInfo);
		$("#loginAccout").text(userInfo.username);
		$(".main-footer a").attr("href","javascript:void(0);").text("遂宁飞行学院管理系统");
		this.hideLoading(500);
	},
	initUserIsLogin:function(){
		
	},
	getNowDate(){
		var date = new Date();
		return date.getFullYear()+'-'+this.parseToDouble((date.getMonth()+1))+'-'+this.parseToDouble(date.getDate());
	},
	getToMorrowDate(){
		var date = new Date() - 24*60*60*1000;
		var tom = new Date(date);
		return tom.getFullYear()+'-'+this.parseToDouble((tom.getMonth()+1))+'-'+this.parseToDouble(tom.getDate());
	},
	parseToDouble(v){
		if(Number(v)){
			return v < 10 ? '0'+v : v;
		}
	}
}
