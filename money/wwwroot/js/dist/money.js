var Money=function(t){var e={};function n(r){if(e[r])return e[r].exports;var a=e[r]={i:r,l:!1,exports:{}};return t[r].call(a.exports,a,a.exports,n),a.l=!0,a.exports}return n.m=t,n.c=e,n.d=function(t,e,r){n.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:r})},n.r=function(t){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},n.t=function(t,e){if(1&e&&(t=n(t)),8&e)return t;if(4&e&&"object"==typeof t&&t&&t.__esModule)return t;var r=Object.create(null);if(n.r(r),Object.defineProperty(r,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var a in t)n.d(r,a,function(e){return t[e]}.bind(null,a));return r},n.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return n.d(e,"a",e),e},n.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},n.p="",n(n.s=0)}([function(t,e,n){(function(t){var e;!function(t){t[t.GET=0]="GET",t[t.POST=1]="POST"}(e||(e={}));var n=t("#add-entry-modal"),r=n.find(".modal-title"),a=n.find(".modal-body"),o=t("#net-worth"),u=function(t,e,n){return alert("XHR ERROR: "+n+", STATUS: "+e)},i=function(n,r,a,o,i){var c={type:e[n],url:r,data:a,cache:!1};t.ajax(c).done(o).fail(i||u)},c=function(){return t(".spinner-border").removeClass("spinner-border-hidden")},d=function(){return t(".spinner-border").addClass("spinner-border-hidden")};t.ajaxSetup({cache:!1}),n.on("click",".btn-primary",(function(r){r.preventDefault(),c();var u=a.find("form");i(e.POST,ADD_ENTRY_URL,u.serialize(),(function(e){d(),e.ok?(e.updated.forEach((function(e){return t("#account-"+e.id).html(e.html)})),o.load(NET_WORTH_URL),n.modal("hide"),setTimeout((function(){return t(".progress-bar").removeClass("updated")}),1e3)):alert("Form Invalid")}))})),t(document).on("click",".btn-add-entry",(function(o){o.preventDefault();var u=t(o.currentTarget),l=parseInt(u.data("accountid"),10),f=u.data("accountname"),s=u.attr("data-categoryid")?parseInt(u.data("categoryid"),10):null,p=u.data("categoryname"),m={accountID:l,categoryID:0!==s?s:null,isCredit:u.attr("data-iscredit")?u.data("iscredit"):null,showCategorySelector:!1,remaining:u.attr("data-remaining")?parseFloat(u.attr("data-remaining")):0};r.html(f+(p?": "+p:"")),c(),i(e.GET,ADD_ENTRY_URL,m,(function(t){a.html(t),n.modal("show"),d()}))})),t(document).on("focus","#Amount",(function(e){var n=t(e.currentTarget);0===parseFloat(n.val())&&n.val("")})),t(document).on("click",".btn-date-preset",(function(e){e.preventDefault(),t("#Date").val(t(e.currentTarget).data("date"))})),t(document).on("click",".btn-amount-preset",(function(e){e.preventDefault(),t("#Amount").val(t(e.currentTarget).data("amount"))}))}).call(this,n(1))},function(t,e){t.exports=jQuery}]);
//# sourceMappingURL=money.js.map