#pragma checksum "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4fec7aac49ad36ef56acbfda3470f70588246548"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Roles_Edit), @"mvc.1.0.view", @"/Views/Roles/Edit.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Roles/Edit.cshtml", typeof(AspNetCore.Views_Roles_Edit))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\_ViewImports.cshtml"
using DynamicEquipmentSystem;

#line default
#line hidden
#line 2 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\_ViewImports.cshtml"
using DynamicEquipmentSystem.Models;

#line default
#line hidden
#line 1 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4fec7aac49ad36ef56acbfda3470f70588246548", @"/Views/Roles/Edit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"31c1e847bfcf79c47245841c42971acf14f9a830", @"/Views/_ViewImports.cshtml")]
    public class Views_Roles_Edit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DynamicEquipmentSystem.ViewModels.ChangeRoleViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(100, 39, true);
            WriteLiteral("\r\n<h2>Изменение ролей для пользователя ");
            EndContext();
            BeginContext(140, 15, false);
#line 4 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
                                Write(Model.UserEmail);

#line default
#line hidden
            EndContext();
            BeginContext(155, 9, true);
            WriteLiteral("</h2>\r\n\r\n");
            EndContext();
            BeginContext(164, 482, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "824a7e1a79084e438cc6d60ca56013f7", async() => {
                BeginContext(202, 40, true);
                WriteLiteral("\r\n    <input type=\"hidden\" name=\"userId\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 242, "\"", 263, 1);
#line 7 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
WriteAttributeValue("", 250, Model.UserId, 250, 13, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(264, 35, true);
                WriteLiteral(" />\r\n    <div class=\"form-group\">\r\n");
                EndContext();
#line 9 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
         foreach (IdentityRole role in Model.AllRoles)
        {

#line default
#line hidden
                BeginContext(366, 47, true);
                WriteLiteral("            <input type=\"checkbox\" name=\"roles\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 413, "\"", 431, 1);
#line 11 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
WriteAttributeValue("", 421, role.Name, 421, 10, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(432, 21, true);
                WriteLiteral("\r\n                   ");
                EndContext();
                BeginContext(455, 64, false);
#line 12 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
               Write(Model.UserRoles.Contains(role.Name) ? "checked=\"checked\"" : "");

#line default
#line hidden
                EndContext();
                BeginContext(520, 3, true);
                WriteLiteral(" />");
                EndContext();
                BeginContext(524, 9, false);
#line 12 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
                                                                                    Write(role.Name);

#line default
#line hidden
                EndContext();
                BeginContext(533, 9, true);
                WriteLiteral(" <br />\r\n");
                EndContext();
#line 13 "C:\Projects\cursach\DynemicEquipmentSystem\DynamicEquipmentSystem\Views\Roles\Edit.cshtml"
            }

#line default
#line hidden
                BeginContext(557, 82, true);
                WriteLiteral("    </div>\r\n    <button type=\"submit\" class=\"btn btn-primary\">Сохранить</button>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DynamicEquipmentSystem.ViewModels.ChangeRoleViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591