Getting started:


Default MVC :
** NB: Current build does not support standard ActionResult returning Controller actions
	ControllerBuilder.Current.SetControllerFactory(
                new UnobtrusiveJsConfiguration()
                .WithConvention(new MvcConvention()).BuildControllerFactory());
        
Custom ControllerBuilder:

	ControllerBuilder.Current.SetControllerFactory(
                new UnobtrusiveJsConfiguration()
                .WithConvention(new MvcConvention())
                .WithControllerFactory(CustomControllerFactory)
					.BuildControllerFactory());
				
				
var windsorControllerFactory = new WindsorControllerFactory(container);
            var objectFactory = new WindsorObjectFactory(container);

            IControllerFactory controllerFactory = FluentMvc.Configure = x => 
            {
                x.ResolveWith(objectFactory);
                x.UsingControllerFactory(windsorControllerFactory);
                
                x.WithFactories()
                    .AddResultFactory<ActionResultFactory>()
                    .AddResultFactory<JsonResultFactory>()
                    .AddResultFactory<ViewResultFactory>(Is.Default());

                x.AddFilter<HandleErrorAttribute>();

                x.AddFilter<AuditTracker>(Apply.For<ClientController>());

                x.AddFilter<NavigationFilter>(Except.When<ExpectsJson>());

                x.AddFilter<CurrentUserDetailsFilter>(Except.When<ExpectsJson>());

                x.AddFilter<SecurityFilter>(Except.For<AccountController>(ac => ac.Register())
                                               .AndFor<AccountController>(ac => ac.LogOn())
                                               .AndFor<AccountController>(ac => ac.LogOn(null))
                                               .AndFor<ContactController>();
                )
            }
            controllerFactory.BuildControllerFactory();

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);