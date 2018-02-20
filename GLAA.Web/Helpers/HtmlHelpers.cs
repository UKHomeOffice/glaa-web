using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GLAA.ViewModels;
using GLAA.ViewModels.Attributes;
using GLAA.ViewModels.Core.Attributes;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace GLAA.Web.Core.Helpers
{

    public static class HtmlHelpers
    {
        public static IHtmlContent ExternalStatusFor<TModel>(this IHtmlHelper<TModel> html, LicenceStatusViewModel status)
        {
            return new HtmlContentBuilder()
                .AppendHtml($"<h3 class='{status.ExternalClassName}'>{status.ExternalDescription}</h3>");
            //var header = new TagBuilder("h3");
            //header.AddCssClass();
            //header.InnerHtml.Append();

            //return new HtmlString(header.ToString());
        }

        public static IHtmlContent InternalStatusFor<TModel>(this IHtmlHelper<TModel> html, LicenceStatusViewModel status)
        {
            return new HtmlContentBuilder()
                .AppendHtml($"<h3 class='{status.InternalClassName}'>{status.InternalStatus}</h3>");
            //var header = new TagBuilder("h3");
            //header.AddCssClass(status.InternalClassName);
            //header.InnerHtml.Append(status.InternalStatus);

            //return new HtmlString(header.ToString());
        }

        public static IHtmlContent PasswordFormGroupFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var control = html.PasswordFor(expression, new { @class = "form-control" });
            return BuildFormGroupForControl(html, expression, control);
        }

        public static IHtmlContent TextFormGroupFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var control = html.TextBoxFor(expression, new { @class = "form-control" });
            return BuildFormGroupForControl(html, expression, control);
        }

        public static IHtmlContent TextAreaFormGroupFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var control = html.TextAreaFor(expression, 6, 60, new { @class = "form-control" });
            return BuildFormGroupForControl(html, expression, control);
        }

        public static IHtmlContent DateFormGroupFor<TModel>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, DateViewModel>> expression)
        {
            return BuildFormGroupForControl(html, expression, html.EditorFor(expression, "_NullableDateTime"));
        }

        public static IHtmlContent TimeSpanFormGroupFor<TModel>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TimeSpanViewModel>> expression)
        {
            return BuildFormGroupForControl(html, expression, html.EditorFor(expression, "_NullableTimeSpan"));
        }

        public static IHtmlContent DropDownFormGroupFor<TModel, TResult>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TResult>> expression, IEnumerable<SelectListItem> items)
        {
            var ddl = html.DropDownListFor(expression, items.OrderBy(i => i.Text).ToArray(), "", new { @class = "form-control"});
            return BuildFormGroupForControl(html, expression, ddl);
        }

        public static IHtmlContent RequiredCheckbox<TModel>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, bool>> expression)
        {
            var fieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var hasErrors = html.ViewData.ModelState[fieldName]?.Errors != null &&
                            html.ViewData.ModelState[fieldName].Errors.Any();

            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider).Metadata;

            //var label = new TagBuilder("span");
            //label.AddCssClass("body-text");
            //label.InnerHtml = metadata.Description;

            return new HtmlContentBuilder()
                .AppendHtml(hasErrors
                    ? "<div class='form-group form-group-error'>"
                    : "<div class='form-group'>")
                .AppendHtml("<fieldset>")
                .AppendHtml($"<legend id='{GenerateLegendId(html, expression)}'>")
                .AppendHtml("<span class=\'error-message\'>")
                .AppendHtml(html.ValidationMessageFor(expression))
                .AppendHtml("</span>")
                .AppendHtml("</legend>")
                .AppendHtml("<div class=\'multiple-choice\'>")
                .AppendHtml(html.CheckBoxFor(expression))
                .AppendHtml(html.LabelFor(expression, metadata.Description, new { }))
                .AppendHtml("</div>")
                .AppendHtml("</fieldset>")
                .AppendHtml("</div>");

            //var errorMsg = new TagBuilder("span");
            //errorMsg.AddCssClass("error-message");
            //errorMsg.InnerHtml.Append(.ToString());

            //var legend = new TagBuilder("legend");
            //legend.Attributes.Add("id", GenerateLegendId(html, expression));
            //legend.InnerHtml.Append($"{errorMsg}");

            //var checkbox = html.CheckBoxFor(expression);

            //var cbx = html.CheckBoxFor(expression);
            //var lbl = ;

            //var div = new TagBuilder("div");
            //div.AddCssClass("multiple-choice");
            //div.InnerHtml.Append($"{lbl}{cbx}");

            //var fieldset = new TagBuilder("fieldset");
            //fieldset.InnerHtml.Append($"{legend}{div}");

            //var parent = new TagBuilder("div");
            //parent.AddCssClass("form-group");
            //parent.InnerHtml.Append(fieldset.ToString());

            //if (hasErrors)
            //{
            //    parent.AddCssClass("form-group-error");
            //}

            //return new HtmlString(parent.ToString());
        }

        public static IHtmlContent CheckBoxFormGroupFor<TModel>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, List<CheckboxListItem>>> expression, IList<CheckboxListItem> values)
        {
            var fieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var hasErrors = html.ViewData.ModelState[fieldName]?.Errors != null &&
                            html.ViewData.ModelState[fieldName].Errors.Any();

            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider).Metadata;

            var htmlBuilder = new HtmlContentBuilder();

            return htmlBuilder
                .AppendHtml(hasErrors
                ? "<div class='form-group form-group-error'>"
                : "<div class='form-group'>")
                .AppendHtml("<fieldset>")
                .AppendHtml($"<legend id='{GenerateLegendId(html, expression)}'>")
                .AppendHtml($"<span class='body-text'>{metadata.Description}</span>")
                .AppendHtml("<span class='error-message'>")
                .AppendHtml(html.ValidationMessageFor(expression))
                .AppendHtml("</span>")
                .AppendHtml("</legend>")
                .AppendHtml(html.CheckboxListFor(expression, values))
                .AppendHtml("</fieldset>")
                .AppendHtml("</div>");
        }

        public static IHtmlContent CheckboxListFor<TModel>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, List<CheckboxListItem>>> expression, IList<CheckboxListItem> values)
        {
            //var sb = new StringBuilder();
            var container = new HtmlContentBuilder();

            for (var i = 0; i < values.Count; i++)
            {
                var indexExpression = Expression.Call(expression.Body, typeof(List<CheckboxListItem>).GetMethod("get_Item"), Expression.Constant(i));

                var checkedAccessExpression = Expression.Property(indexExpression, typeof(CheckboxListItem), "Checked");
                var checkedLambda = Expression.Lambda<Func<TModel, bool>>(checkedAccessExpression, expression.Parameters[0]);

                var idAccessExpression = Expression.Property(indexExpression, typeof(CheckboxListItem), "Id");
                var idLambda = Expression.Lambda<Func<TModel, int>>(idAccessExpression, expression.Parameters[0]);

                var nameAccessExpression = Expression.Property(indexExpression, typeof(CheckboxListItem), "Name");
                var nameLambda = Expression.Lambda<Func<TModel, string>>(nameAccessExpression, expression.Parameters[0]);

                var idHid = html.HiddenFor(idLambda);
                var nameHid = html.HiddenFor(nameLambda);
                var cbx = html.CheckBoxFor(checkedLambda);
                var lbl = html.LabelFor(checkedLambda, values[i].Name);

                var div = new TagBuilder("div");
                div.AddCssClass("multiple-choice");
                div.InnerHtml
                    .AppendHtml(idHid)
                    .AppendHtml(nameHid)
                    .AppendHtml(cbx)
                    .AppendHtml(lbl);

                container.AppendHtml(div);
            }

            return container;
        }
        private static void GetItemForRadioValues<TModel, TValue>(IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IList<SelectListItem> values,
            IHtmlContentBuilder builder)
        {
            foreach (var item in values)
            {
                var rbx = html.RadioButtonFor(expression, item.Value);
                var lbl = html.Label(item.Text);
                var div = new TagBuilder("div");
                div.AddCssClass("multiple-choice");
                div.InnerHtml.AppendHtml(rbx);
                div.InnerHtml.AppendHtml(lbl);

                builder.AppendHtml(div);
            }
        }

        private static IHtmlContentBuilder SetupRadioButtonGroup<TModel, TValue>(IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression,
            string subHeading)
        {
            var fieldName =
                html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var hasErrors = html.ViewData.ModelState[fieldName]?.Errors != null &&
                            html.ViewData.ModelState[fieldName].Errors.Any();

            var builder = new HtmlContentBuilder()
                .AppendHtml(hasErrors
                    ? "<div class='form-group form-group-error'>"
                    : "<div class='form-group'>")
                .AppendHtml("<fieldset>")
                .AppendHtml($"<legend id='{GenerateLegendId(html, expression)}'>")
                .AppendHtml(!string.IsNullOrEmpty(subHeading)
                    ? $"<span class='body-text'>{subHeading}</span>"
                    : string.Empty)
                .AppendHtml(html.LabelWithHintFor(expression))
                .AppendHtml("<span class=\'error-message\'>")
                .AppendHtml(html.ValidationMessageFor(expression))
                .AppendHtml("</span>");
            return builder;
        }

        public static IHtmlContent RadioButtonFormGroupFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IList<SelectListItem> values, string subHeading = null)
        {
            var builder = SetupRadioButtonGroup(html, expression, subHeading);

            GetItemForRadioValues(html, expression, values, builder);

            return builder.AppendHtml("</fieldset>").AppendHtml("</div>");

        }

        private static void GetItemForRadioValuesEnum<TModel, TValue>(IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IHtmlContentBuilder builder)
        {
            //By checking the underlying type, it means we can support nullable and non nullable enum values in the model.
            var enumType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            // values are extracted directly out of the enum.
            foreach (var item in Enum.GetValues(enumType))
            {
                var rbx = html.RadioButtonFor(expression, item.ToString());
                var lbl = html.Label(item.ToString());
                var div = new TagBuilder("div");
                div.AddCssClass("multiple-choice");
                div.InnerHtml.AppendHtml(rbx);
                div.InnerHtml.AppendHtml(lbl);

                builder.AppendHtml(div);
            }
        }

        public static IHtmlContent RadioButtonFormGroupForEnum<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string subHeading = null)
        {
            var builder = SetupRadioButtonGroup(html, expression, subHeading);

            GetItemForRadioValuesEnum(html, expression, builder);

            return builder.AppendHtml("</fieldset>").AppendHtml("</div>");
        }

        public static IHtmlContent RadioButtonForEnum<TModel, TEnumValue, TListValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TEnumValue>> expression, string value, List<TListValue> checkBoxListItems)
            where TListValue : ICheckboxList<TEnumValue>
        {
            var checkBoxListItem = checkBoxListItems.SingleOrDefault(x => x.Name == value);

            return checkBoxListItem != null ? html.RadioButtonFor(expression, checkBoxListItem.EnumMappedTo) : null;
        }

        public static IHtmlContent LabelWithHintFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData, html.MetadataProvider).Metadata;
            var resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            var optionalText = metadata.IsRequired ||
                IsCompared(expression) ||
                IsHiddenOptional(expression) ||
                IsDateRequired(expression) ||
                IsRequiredIf(expression) ||
                IsAssertThat(expression) ||
                IsRequireTrue(expression) ||
                IsRequiredForShellfish(expression, html.ViewData.Model as IShellfishSection) ||
                IsRequiredIfUk(expression, html.ViewData.Model as IUkOnly) ? string.Empty : "(optional)";

            return new HtmlContentBuilder()
                .AppendHtml(
                    $"<label class='form-label'>{resolvedLabelText} {optionalText} " +
                    $"<span class='form-hint'>{metadata.Description}</span></label>");
        }

        private static IHtmlContent BuildFormGroupForControl<TModel, TValue>(IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IHtmlContent control)
        {
            var fieldName =
                html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var hasErrors = html.ViewData.ModelState[fieldName]?.Errors != null &&
                            html.ViewData.ModelState[fieldName].Errors.Any();

            return new HtmlContentBuilder()
                .AppendHtml(hasErrors
                    ? "<div class='form-group form-group-error'>"
                    : "<div class='form-group'>")
                .AppendHtml("<fieldset>")
                .AppendHtml($"<legend id='{GenerateLegendId(html, expression)}'>")
                .AppendHtml(html.LabelWithHintFor(expression))
                .AppendHtml("<span class=\'error-message\'>")
                .AppendHtml(html.ValidationMessageFor(expression))
                .AppendHtml("</span>")
                .AppendHtml("</legend>")
                .AppendHtml(control)
                .AppendHtml("</fieldset>")
                .AppendHtml("</div>");

            //var legend = new TagBuilder("legend");
            //legend.Attributes.Add("id", GenerateLegendId(html, expression));

            //var error = new TagBuilder("span");
            //error.AddCssClass("error-message");

            //error.InnerHtml.Append(html.ValidationMessageFor(expression).ToString());
            //legend.InnerHtml.Append($"{html.LabelWithHintFor(expression)} {error}");

            //var fieldset = new TagBuilder("fieldset");
            //fieldset.InnerHtml.Append($"{legend} {control}");

            //var parent = new TagBuilder("div");
            //parent.AddCssClass("form-group");
            //parent.InnerHtml.Append(fieldset.ToString());
            //if (hasErrors)
            //{
            //    parent.AddCssClass("form-group-error");
            //}

            //return new HtmlString(parent.ToString());
        }

        private static bool IsRequireTrue<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<RequireTrueAttribute>() != null;
        }

        private static bool IsRequiredIf<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<RequiredIfAttribute>() != null;
        }

        private static bool IsRequiredForShellfish<TModel, TValue>(Expression<Func<TModel, TValue>> expression, IShellfishSection model)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<RequiredForShellfishAttribute>() != null && model.IsShellfish;
        }

        private static bool IsRequiredIfUk<TModel, TValue>(Expression<Func<TModel, TValue>> expression, IUkOnly model)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<RequiredIfUkAddressAttribute>() != null && model.IsUk;
        }

        private static bool IsAssertThat<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<AssertThatAttribute>() != null;
        }

        private static bool IsDateRequired<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<DateRequiredAttribute>() != null;
        }

        private static bool IsCompared<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<CompareAttribute>() != null;
        }

        private static bool IsHiddenOptional<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<HiddenOptionalAttribute>() != null;
        }

        private static bool HasUIHint<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetCustomAttribute<UIHintAttribute>() != null;
        }

        private static string GenerateLegendId<TModel, TValue>(IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var htmlFieldId =
                html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            return GenerateLegendId(htmlFieldId);
        }

        private static string GenerateLegendId(string fieldName)
        {
            return $"legend_{fieldName}";
        }
    }
}

