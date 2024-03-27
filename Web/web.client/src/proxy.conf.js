const { env } = require('process');

const target = env.SSL_PORT ? `https://localhost:${env.SSL_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:7188';

const PROXY_CONFIG = [
  {
    context: [
      "/api/**",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
