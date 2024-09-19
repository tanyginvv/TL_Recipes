**Книга рецептов ( Recipes )**

Приложение для управления и просмотра информации о рецептах еды позволяет пользователям добавлять, редактировать и удалять рецепты, а также загружать изображения для них. Список рецептов можно просматривать, смотреть время приготовления и тэги. Пользователи могут авторизоваться для создания и сохранения рецептов, а также ставить лайки понравившимся блюдам. Дополнительно реализован поиск рецептов по ключевым словам и тэгам для удобства навигации

**Макет**: https://www.figma.com/design/e0kFpmYzRpgNcQwuowtrCL/Recipes-_-Cooking-Website-Homepage?node-id=0-1&node-type=canvas&t=Z8cQTpzDHAdXnFAa-0

**Презентация**: https://docs.google.com/presentation/d/1PuYVefAwJVnPRbr6yBqjsieEsaKiI_TA/edit#slide=id.p1

**Backend**: .Net 8.0, Entity Framework Core, JWT-токены

**СУБД**: MSSQL

**Frontend**: React + ts, Vite, Zustand

**Контейнеризация**: Docker

**ER-диаграмма**:![OE1iza-Xdn0](https://github.com/user-attachments/assets/d335e341-0770-4b1a-af2d-cae73219587e)

**Запуск**: 

- ветка master через visual studio

- ветка docker через docker-compose в vs

Backend настроен на возврат статических файлов из wwwroot.

Для создания статических файлов нужно перейти в папку ../frontend/Recipes_frontend и выполнить команду **npm run build**, содержимое папки dist переместить в папку wwwroot.
