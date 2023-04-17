# Minishop
![Главная страница](https://github.com/XeeRooX/minishop/blob/master/minishop/readmeImages/1.png "Главная")
## Установка и запуск
Для запуска Docker контейнера используйте следующие команды:
```
git clone https://github.com/XeeRooX/minishop.git
cd ./minishop
docker compose up
```
Далее в браузере перейдите по следующему адресу:
``` https://localhost:5001 ```
## Настройки контейнера
Чтобы изменить логин и пароль доступа к аккаунту администратора, можно задать переменные окружения:
```
admin:email=admin@gm.co
admin:password=1234
```