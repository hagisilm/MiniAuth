import{u as g,r as i,o as y,a as n,c as a,b as e,t as o,F as _,d as C,w as c,v,e as V,p as E,f as R}from"./index-WEokU1c-.js";import{_ as M,s as u}from"./_plugin-vue_export-helper--XdcB9Re.js";const b=d=>(E("data-v-c0055499"),d=d(),R(),d),S={class:"scrollable-container"},$={class:"table"},I={class:"form-check form-switch"},L=["disabled","onUpdate:modelValue"],T={class:"form-check form-switch"},U=["disabled","onUpdate:modelValue"],x=["disabled","onUpdate:modelValue"],B=b(()=>e("option",null,null,-1)),H=["value"],N=["disabled","onClick"],D=b(()=>e("svg",{width:"20px",height:"20px",viewBox:"0 0 24 24",fill:"none",xmlns:"http://www.w3.org/2000/svg"},[e("g",{id:"System / Save"},[e("path",{id:"Vector",d:"M17 21.0002L7 21M17 21.0002L17.8031 21C18.921 21 19.48 21 19.9074 20.7822C20.2837 20.5905 20.5905 20.2843 20.7822 19.908C21 19.4806 21 18.921 21 17.8031V9.21955C21 8.77072 21 8.54521 20.9521 8.33105C20.9095 8.14 20.8393 7.95652 20.7432 7.78595C20.6366 7.59674 20.487 7.43055 20.1929 7.10378L17.4377 4.04241C17.0969 3.66374 16.9242 3.47181 16.7168 3.33398C16.5303 3.21 16.3242 3.11858 16.1073 3.06287C15.8625 3 15.5998 3 15.075 3H6.2002C5.08009 3 4.51962 3 4.0918 3.21799C3.71547 3.40973 3.40973 3.71547 3.21799 4.0918C3 4.51962 3 5.08009 3 6.2002V17.8002C3 18.9203 3 19.4796 3.21799 19.9074C3.40973 20.2837 3.71547 20.5905 4.0918 20.7822C4.5192 21 5.07899 21 6.19691 21H7M17 21.0002V17.1969C17 16.079 17 15.5192 16.7822 15.0918C16.5905 14.7155 16.2837 14.4097 15.9074 14.218C15.4796 14 14.9203 14 13.8002 14H10.2002C9.08009 14 8.51962 14 8.0918 14.218C7.71547 14.4097 7.40973 14.7155 7.21799 15.0918C7 15.5196 7 16.0801 7 17.2002V21M15 7H9",stroke:"#000000","stroke-width":"2","stroke-linecap":"round","stroke-linejoin":"round"})])],-1)),P=[D],A={__name:"EndpointsView",setup(d){const{t:p}=g();i("EndPoints");const r=i([]),h=i([]),m=async()=>{r.value=await u.get("api/getAllEndpoints"),h.value=await u.get("api/getRoles")},f=async l=>{confirm(p("please_confirm"))&&await u.post("api/saveEndpoint",l).then(()=>{alert(p("updated_successfully"))})};return y(async()=>{await m()}),(l,F)=>(n(),a("div",S,[e("table",$,[e("thead",null,[e("tr",null,[e("th",null,o(l.$t("Name")),1),e("th",null,o(l.$t("Route")),1),e("th",null,o(l.$t("Redirect")),1),e("th",null,o(l.$t("Enable")),1),e("th",null,o(l.$t("Roles")),1),e("th",null,o(l.$t("Action")),1)])]),e("tbody",null,[(n(!0),a(_,null,C(r.value,(t,w)=>(n(),a("tr",{key:w},[e("td",null,o(t.Name),1),e("td",null,o(t.Route),1),e("td",null,[e("div",I,[c(e("input",{disabled:t.Type=="miniauth",class:"form-check-input",type:"checkbox","onUpdate:modelValue":s=>t.RedirectToLoginPage=s},null,8,L),[[v,t.RedirectToLoginPage]])])]),e("td",null,[e("div",T,[c(e("input",{disabled:t.Type=="miniauth",class:"form-check-input",type:"checkbox","onUpdate:modelValue":s=>t.Enable=s},null,8,U),[[v,t.Enable]])])]),e("td",null,[c(e("select",{disabled:t.Type=="miniauth",multiple:"","onUpdate:modelValue":s=>t.Roles=s,class:"resizable",style:{height:"25px"}},[B,(n(!0),a(_,null,C(h.value,(s,k)=>(n(),a("option",{value:s.Id,key:k},o(s.Name),9,H))),128))],8,x),[[V,t.Roles]])]),e("td",null,[e("button",{disabled:t.Type=="miniauth",class:"btn",onClick:s=>f(t)},P,8,N)])]))),128))])])]))}},q=M(A,[["__scopeId","data-v-c0055499"]]);export{q as default};