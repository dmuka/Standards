const { createProxyMiddleware } = require('http-proxy-middleware')

// urls for proxy
const context = [
  '/swagger',
  '/api'
]

const { env } = require('process')

// values from env file (backend host)
const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:3000'

// error handler
const onError = (err, req, resp, target) => {
  console.error(`${err.message}`)
}

module.exports = function (app) {
  const appProxy = createProxyMiddleware(context, {
    target,
    onError,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  })

  app.use(appProxy)
}
