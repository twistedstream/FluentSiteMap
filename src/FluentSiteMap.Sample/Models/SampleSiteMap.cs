using FluentSiteMap.Builders;
using FluentSiteMap.Filters;

namespace FluentSiteMap.Sample.Models
{
    public class SampleSiteMap
        : SiteMap
    {
        public SampleSiteMap(IProductRepository productRepository)
        {
            Root =
                Node()
                    .WithTitle("Home")
                    .WithDescription("Welcome to Foo.com!")
                    .ForController("Home").ForAction("Index").WithUrlFromMvc()
                    .WithChildren(
                        Node()
                            .WithTitle("About Us").WithDescriptionSameAsTitle()
                            .ForController("Home").ForAction("About").WithUrlFromMvc(),
                        Node()
                            .WithTitle("Account")
                            .ForController("Account")
                            .WithChildren(
                                Node()
                                    .WithTitle("Sign In").WithDescriptionSameAsTitle()
                                    .ForAction("Login").WithUrlFromMvc()
                                    .IfNotAuthenticated(),
                                Node()
                                    .WithTitle("Sign Out").WithDescriptionSameAsTitle()
                                    .ForAction("Logout").WithUrlFromMvc()
                                    .IfAuthenticated()),
                        Node()
                            .WithTitle("Products").WithDescriptionSameAsTitle()
                            .ForController("Products").ForAction("Index").WithUrlFromMvc()
                            .WithChildren(productRepository.FetchProducts(),
                                          (p, b) => b
                                                        .WithTitle(p.Name)
                                                        .WithDescription(p.Description)
                                                        .ForAction("View").WithUrlFromMvc(new {id = p.Id})
                                                        .WithMetadata("MenuImage", p.ImageName)),
                        Node()
                            .WithTitle("Administration").WithDescriptionSameAsTitle()
                            .ForController("Admin").ForAction("Index").WithUrlFromMvc()
                            .IfInRole("Admin"));
        }
    }
}