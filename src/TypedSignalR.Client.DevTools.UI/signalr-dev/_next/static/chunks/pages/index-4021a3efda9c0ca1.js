(self.webpackChunk_N_E=self.webpackChunk_N_E||[]).push([[405],{8312:function(e,t,s){(window.__NEXT_P=window.__NEXT_P||[]).push(["/",function(){return s(2129)}])},2129:function(e,t,s){"use strict";s.r(t),s.d(t,{default:function(){return P}});var a=s(5893),l=s(7294);let n=e=>(0,a.jsx)(a.Fragment,{children:e.text&&(0,a.jsx)("pre",{className:"has-background-dark has-text-white pre-code",children:(0,a.jsx)("code",{className:"wrap-anywhere",children:e.text})})}),r=e=>{let[t,s]=(0,l.useState)(""),r=e.options,c=e.message;return(0,l.useEffect)(()=>{let e=JSON.stringify(c.content,null,4);s(e)},[c,r]),(0,a.jsxs)("div",{className:"box m-2",children:[(0,a.jsx)("div",{className:"is-large",children:(0,a.jsx)("strong",{children:e.message.methodName})}),(0,a.jsx)("div",{className:"m-3",children:r.indent?(0,a.jsx)(n,{text:t}):(0,a.jsx)("div",{className:"wrap-anywhere",children:t})})]})},c=e=>{let[t,s]=(0,l.useState)(!1),n=e.messages.map((e,s)=>(0,a.jsx)("div",{children:(0,a.jsx)(r,{message:{methodName:e.methodName,content:e.content},options:{indent:t}})},s)).reverse();return(0,a.jsxs)("div",{className:"h-screen vertical-container",children:[(0,a.jsx)("div",{className:"box",children:(0,a.jsxs)("div",{className:"level",children:[(0,a.jsx)("div",{className:"level-left",children:(0,a.jsx)("div",{className:"is-large",children:(0,a.jsx)("strong",{children:"Receive Message Log"})})}),(0,a.jsx)("div",{className:"level-right",children:(0,a.jsx)("div",{className:"content",children:(0,a.jsxs)("label",{className:"checkbox",children:[(0,a.jsx)("input",{type:"checkbox",onChange:e=>{s(e.target.checked)}}),"indent"]})})})]})}),(0,a.jsx)("div",{className:"h-max w-max",children:(0,a.jsx)("div",{className:"has-background-light scrollarea h-max",children:n})})]})};var i=s(7830),o=s(239);let d=e=>{let t=e.toLowerCase();return"true"===t},m=e=>Number(e),h=e=>new Date(e),u=e=>e?JSON.parse(e):null,x={bool:d,char:e=>e,byte:m,sbyte:m,decimal:m,double:m,float:m,int:m,uint:m,long:m,ulong:m,short:m,ushort:m,"byte[]":e=>e,string:e=>e,"global::System.Uri":e=>e,"global::System.Guid":e=>e,"global::System.DateTime":h,"global::System.DateTimeOffset":h},g=e=>e in x?x[e]:u,j=e=>{let t=e.method,s=e.hubConnection,[r,c]=(0,l.useState)([]),[i,o]=(0,l.useState)(null);(0,l.useEffect)(()=>{let t=Array(e.method.parameters.length);c(t)},[e.method]);let d=(0,l.useCallback)(()=>{let e=async()=>{try{let e=await (null==s?void 0:s.invoke(t.methodName,...r)),a="global::System.Threading.Tasks.Task"===t.returnType?"Invocation completed.":JSON.stringify(e,null,4);o(a)}catch(e){o("".concat(e))}};e()},[r,t,s,o]),m=e.method.parameters.map((e,t)=>(0,a.jsx)(p,{name:e.name,typeName:e.typeName,index:t,setArgs:c},t));return(0,a.jsxs)("div",{className:"box",children:[(0,a.jsx)("label",{className:"label is-large",children:t.methodName}),m,(0,a.jsx)("button",{className:"button is-info",disabled:!s,onClick:e=>d(),children:"Invoke"}),i&&(0,a.jsxs)(a.Fragment,{children:[(0,a.jsxs)("h5",{className:"m-3",children:["response : ",t.returnType]}),(0,a.jsx)(n,{text:i})]})]})},p=e=>{let[t,s]=(0,l.useState)(null),{name:n,typeName:r,index:c,setArgs:i}=e;return(0,a.jsxs)("div",{className:"field",children:[(0,a.jsxs)("label",{className:"label",children:[n," : ",r]}),(0,a.jsx)("div",{className:"control",children:(0,a.jsx)("textarea",{className:t?"input is-danger":"input",placeholder:r,onBlur:e=>{try{let t=e.target.value,a=g(r),l=a(t);i(e=>(e[c]=l,[...e])),s(void 0)}catch(e){i(e=>(e[c]=null,[...e])),s("".concat(e))}}})}),t&&(0,a.jsx)("p",{className:"help is-danger",children:"".concat(t)})]})},b=e=>{let t=e.method,s=e.hubConnection,r=e.setStreamMessage,[c,i]=(0,l.useState)([]),[o,d]=(0,l.useState)(null);(0,l.useEffect)(()=>{let t=Array(e.method.parameters.length);i(t)},[e.method]);let m=(0,l.useCallback)(()=>{let e=async()=>{console.log("=== ServerToClientStreamingHubMethodInvoker ==="),console.log(c);let e=v(t,c);console.log(e);try{null==s||s.stream(t.methodName,...e).subscribe({next:e=>{r({methodName:"".concat(t.methodName," : OnNext"),content:e})},complete:()=>{r({methodName:"".concat(t.methodName," : OnCompleted"),content:""}),d("Stream stoped.")},error:e=>{r({methodName:"".concat(t.methodName," : OnError"),content:"".concat(e)}),d("Stream stoped.")}}),d("Stream started.")}catch(e){d("".concat(e))}};e()},[c,t,s,d,r]),h=e.method.parameters.map((e,t)=>(0,a.jsx)(N,{name:e.name,typeName:e.typeName,index:t,setArgs:i},t));return(0,a.jsxs)("div",{className:"box",children:[(0,a.jsx)("label",{className:"label is-large",children:t.methodName}),h,(0,a.jsx)("button",{className:"button is-info",disabled:!s,onClick:e=>m(),children:"Invoke"}),o&&(0,a.jsxs)(a.Fragment,{children:[(0,a.jsxs)("h5",{className:"m-3",children:["response : ",t.returnType]}),(0,a.jsx)(n,{text:o})]})]})},N=e=>{let[t,s]=(0,l.useState)(null),{name:n,typeName:r,index:c,setArgs:i}=e;return(0,a.jsxs)("div",{className:"field",children:[(0,a.jsxs)("label",{className:"label",children:[n," : ",r]}),(0,a.jsx)("div",{className:"control",children:(0,a.jsx)("textarea",{className:t?"input is-danger":"input",placeholder:r,disabled:"global::System.Threading.CancellationToken"===r,onBlur:e=>{try{let t=e.target.value,a=g(r),l=a(t);i(e=>(e[c]=l,[...e])),s(void 0)}catch(e){i(e=>(e[c]=null,[...e])),s("".concat(e))}}})}),t&&(0,a.jsx)("p",{className:"help is-danger",children:"".concat(t)})]})},v=(e,t)=>{let s=e.parameters.map(e=>e.typeName).indexOf("global::System.Threading.CancellationToken");if(s<0)return t;let a=[...t];return a.splice(s,1),a};var y=s(5403);let f=e=>{let t=e.method,s=e.hubConnection,[r,c]=(0,l.useState)([]),[i,o]=(0,l.useState)(null),[d,m]=(0,l.useState)(null);(0,l.useEffect)(()=>{let t=Array(e.method.parameters.length);c(t)},[e.method]);let h=(0,l.useCallback)(()=>{let e=async()=>{console.log(r);let e=new y.x;o(e);try{let a=A(t,r,e);await (null==s?void 0:s.send(t.methodName,...a)),m("Stream started.")}catch(t){m("".concat(t)),o(null),null==e||e.error(t)}};e()},[r,t,s]),u=w(t),x=e.method.parameters.map((e,t)=>(0,a.jsx)(S,{name:e.name,typeName:e.typeName,index:t,setArgs:c},t));return(0,a.jsxs)("div",{className:"box",children:[(0,a.jsx)("label",{className:"label is-large",children:t.methodName}),x,(0,a.jsx)("button",{className:"button is-info",disabled:!s||!!i,onClick:e=>h(),children:"Invoke"}),(0,a.jsx)(R,{typeName:t.parameters[u].typeName,name:t.parameters[u].name,subject:i}),(0,a.jsx)("button",{className:"button is-info ml-3",disabled:!i,onClick:e=>{null==i||i.complete(),o(null),m("Stream stopped.")},children:"Complete"}),(0,a.jsx)("button",{className:"button is-info ml-3",disabled:!i,onClick:e=>{null==i||i.error("The stream has been canceled."),o(null),m("Stream stopped.")},children:"Cancel"}),d&&(0,a.jsxs)(a.Fragment,{children:[(0,a.jsxs)("h5",{className:"m-3",children:["response : ",t.returnType]}),(0,a.jsx)(n,{text:d})]})]})},S=e=>{let[t,s]=(0,l.useState)(null),{name:n,typeName:r,index:c,setArgs:i}=e;return(0,a.jsxs)("div",{className:"field",children:[(0,a.jsxs)("label",{className:"label",children:[n," : ",r]}),(0,a.jsx)("div",{className:"control",children:(0,a.jsx)("textarea",{className:t?"input is-danger":"input",placeholder:r,disabled:T(r),onBlur:e=>{try{let t=e.target.value,a=g(r),l=a(t);i(e=>(e[c]=l,[...e])),s(void 0)}catch(e){i(e=>(e[c]=null,[...e])),s("".concat(e))}}})}),t&&(0,a.jsx)("p",{className:"help is-danger",children:"".concat(t)})]})},C="global::System.Collections.Generic.IAsyncEnumerable",k="global::System.Threading.Channels.ChannelReader",T=e=>!!(e.startsWith(C)||e.startsWith(k)),w=e=>{for(let t=0;t<e.parameters.length;t++)if(T(e.parameters[t].typeName))return t;return -1},A=(e,t,s)=>{let a=w(e);return a<0||(t[a]=s),t},R=e=>{let[t,s]=(0,l.useState)(null),[n,r]=(0,l.useState)(null),{name:c,typeName:i,subject:o}=e;return(0,a.jsxs)(a.Fragment,{children:[(0,a.jsxs)("div",{className:"field",children:[(0,a.jsxs)("label",{className:"label",children:[c," : ",i]}),(0,a.jsx)("div",{className:"control",children:(0,a.jsx)("textarea",{className:n?"input is-danger":"input",placeholder:i,disabled:!o,onBlur:e=>{try{let t=e.target.value,a=g(E(i)),l=a(t);s(l),r(void 0)}catch(e){s(null),r("".concat(e))}}})}),n&&(0,a.jsx)("p",{className:"help is-danger",children:"".concat(n)})]}),(0,a.jsx)("button",{className:"button is-info",disabled:!o,onClick:e=>null==o?void 0:o.next(t),children:"Next"})]})},E=e=>{if(e.startsWith(C)){let t=C.length+1,s=e.length-1;return e.substring(t,s)}if(e.startsWith(k)){let t=k.length+1,s=e.length-1;return e.substring(t,s)}return""},_=e=>{let{method:t,hubConnection:s,setReceivedMessage:l}=e;return I(t)?(0,a.jsx)(b,{method:t,hubConnection:s,setStreamMessage:l}):W(t)?(0,a.jsx)(f,{method:t,hubConnection:s}):(0,a.jsx)(j,{method:t,hubConnection:s})},I=e=>!!(e.returnType.startsWith("global::System.Collections.Generic.IAsyncEnumerable")||e.returnType.startsWith("global::System.Threading.Tasks.Task<global::System.Collections.Generic.IAsyncEnumerable")||e.returnType.startsWith("global::System.Threading.Tasks.Task<global::System.Threading.Channels.ChannelReader")),W=e=>{for(let t of e.parameters)if(t.typeName.startsWith("global::System.Collections.Generic.IAsyncEnumerable")||t.typeName.startsWith("global::System.Threading.Channels.ChannelReader"))return!0;return!1},M=["has-background-info-light","has-background-primary-light","has-background-danger-light"],O=e=>{let t=e.service,s=e.setReceivedMessage,[r,c]=(0,l.useState)(null),[d,m]=(0,l.useState)(null),[h,u]=(0,l.useState)(null),x=(0,l.useCallback)(()=>{let e=async()=>{if(r&&await r.stop(),t.isAuthRequired&&!h){m("Please input JWT.");return}let e=t.isAuthRequired?{accessTokenFactory:()=>h}:{},a=new i.s().withUrl(t.path,e).withAutomaticReconnect().configureLogging(o.i.Information).build();for(let e of t.receiverType.methods)a.on(e.methodName,function(){for(var t=arguments.length,a=Array(t),l=0;l<t;l++)a[l]=arguments[l];return s({methodName:e.methodName,content:a})});try{await a.start(),c(a),m("Connection succeeded.")}catch(e){m("Exception occurred:\n ".concat(e))}};e()},[r,h,t,c,m,s]);return(0,a.jsx)("div",{className:"container is-max-widescreen ".concat(M[e.index%3]," my-6"),children:(0,a.jsxs)("div",{className:"content p-5",children:[(0,a.jsx)(F,{service:t}),(0,a.jsx)(q,{isAuthRequired:t.isAuthRequired,hubConnection:r,setJwt:u}),(0,a.jsx)("button",{className:"button is-warning my-5",onClick:()=>x(),children:"Connect to Hub"}),(0,a.jsx)(n,{text:d}),(0,a.jsx)(J,{methods:t.hubType.methods,hubConnection:r,setReceivedMessage:s})]})})},F=e=>{let t=e.service;return(0,a.jsxs)(a.Fragment,{children:[(0,a.jsx)("h1",{className:"is-size-2",children:t.name}),(0,a.jsxs)("ul",{children:[(0,a.jsxs)("li",{children:["Hub Type: ",(0,a.jsx)("strong",{children:t.hubType.interfaceName})]}),(0,a.jsxs)("li",{children:["Receiver Type: ",(0,a.jsx)("strong",{children:t.receiverType.interfaceName})]}),(0,a.jsxs)("li",{children:["Path: ",(0,a.jsx)("strong",{children:t.path})]}),t.isAuthRequired&&(0,a.jsx)("li",{children:(0,a.jsx)("strong",{children:"Authorization Required"})})]})]})},q=e=>{let{isAuthRequired:t,hubConnection:s,setJwt:l}=e;return(0,a.jsx)(a.Fragment,{children:t&&(0,a.jsx)("div",{className:"field",children:(0,a.jsx)("p",{className:"control",children:(0,a.jsx)("input",{className:"input",type:"password",placeholder:"Input JWT",onBlur:e=>{let t=e.target.value;l(t)},disabled:!!s})})})})},J=e=>{let{methods:t,hubConnection:s,setReceivedMessage:l}=e,n=t.map((e,t)=>(0,a.jsx)(_,{method:e,hubConnection:s,setReceivedMessage:l},t));return(0,a.jsx)(a.Fragment,{children:n})},B=e=>{let t=e.services.map((t,s)=>(0,a.jsx)(O,{service:t,index:s,setReceivedMessage:e.setReceivedMessage},s));return(0,a.jsx)("div",{className:"m-5",children:t})},G=()=>{let[e,t]=(0,l.useState)([]),[s,n]=(0,l.useState)([]),r=(0,l.useCallback)(e=>t(t=>[...t,e]),[]);return(0,l.useEffect)(()=>{let e=async()=>{let e=await fetch("./spec.json"),t=await e.json();console.log(t),n(t)};e()},[]),(0,a.jsxs)("div",{className:"columns h-screen w-screen",children:[(0,a.jsx)("div",{className:"column is-two-thirds scrollarea",children:(0,a.jsx)(B,{services:s,setReceivedMessage:r})}),(0,a.jsx)("div",{className:"column is-one-thirds",children:(0,a.jsx)("div",{children:(0,a.jsx)(c,{messages:e})})})]})};var P=G}},function(e){e.O(0,[830,774,888,179],function(){return e(e.s=8312)}),_N_E=e.O()}]);