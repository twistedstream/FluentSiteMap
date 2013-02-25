﻿using FluentSiteMap.Builders;
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
                            .ForAction("About").WithUrlFromMvc(),
                        Node()
                            .WithTitle("Account")
                            .ForController("Account")
                            .HiddenInMenu()
                            .HiddenInBreadCrumbs()
                            .WithChildren(
                                Node()
                                    .WithTitle("Sign In").WithDescriptionSameAsTitle()
                                    .ForAction("LogOn").WithUrlFromMvc()
                                    .IfNotAuthenticated(),
                                Node()
                                    .WithTitle("Sign Out").WithDescriptionSameAsTitle()
                                    .ForAction("LogOff").WithUrlFromMvc()
                                    .IfAuthenticated(),
                                Node()
                                    .WithTitle("Register").WithDescriptionSameAsTitle()
                                    .ForAction("Register").WithUrlFromMvc()),
                        Node()
                            .WithTitle("Products").WithDescriptionSameAsTitle()
                            .ForController("Products").ForAction("Index").WithUrlFromMvc()
                            .WithChildren(productRepository.FetchProducts(),
                                          (p, b) => b
                                                        .WithTitle(p.Name)
                                                        .WithDescription(p.Description)
                                                        .ForAction("View").WithUrlFromMvc(new {id = p.Id})
                                                        .WithMetadata("Price", p.Price)),
                        Node()
                            .WithTitle("Site Map").WithDescriptionSameAsTitle()
                            .ForAction("SiteMap").WithUrlFromMvc(),
                        Node()
                            .WithTitle("Google").WithDescriptionSameAsTitle()
                            .WithUrl("http://google.com")
                            .WithTargetThatOpensNewWindow(),
                        Node()
                            .WithTitle("Administration").WithDescriptionSameAsTitle()
                            .ForController("Admin").ForAction("Index").WithUrlFromMvc()
                            .IfInRole("Admin"));
        }
    }
}