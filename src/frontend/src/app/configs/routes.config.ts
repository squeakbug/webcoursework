export const RoutesConfig = {
  routes: {
    error404: 'error404',
    auth: {
      root: 'auth',
      signUp: 'signup',
      logIn: 'login'
    },
    converter: {
      root: 'converter',
      configurations: 'configurations',
      main: 'main'
    }
  }
};

export const FullRoutes = {
  error404: `/${RoutesConfig.routes.error404}`,
  login: `/${RoutesConfig.routes.auth.root}/${RoutesConfig.routes.auth.logIn}`,
  signup: `/${RoutesConfig.routes.auth.root}/${RoutesConfig.routes.auth.signUp}`,
  configurations: `/${RoutesConfig.routes.converter.root}/${RoutesConfig.routes.converter.configurations}`,
  main: `/${RoutesConfig.routes.converter.root}/${RoutesConfig.routes.converter.main}`
}
