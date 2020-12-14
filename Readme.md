3 Проекта: 
1. IS 4 
2. Mvc c refit + errorHandlers + serilog + oidc 
3. Webapi c oidc + postgres + problem + ErrorHandlingMiddleware+ quartz

Добавлены Integration tests, которые работают с WebApi+ AuthorizeAttribute + IS4
1. Для IntegrationTestExample Предварительно IS4 должен быть запущен явно
2. Для IntegrationTestExample2 mock dbContext и создания токена в схеме Test

Ваня предлагает получать токен с Production среды и тестировать ее

База Products и IS4 переведены на Postgres
