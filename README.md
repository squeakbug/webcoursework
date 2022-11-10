## Решаемая проблема
Конвертация шрифта из формата TrueType в формат библиотеки PEG (Portable Embedded GUI)

## Функциональные требования
- настройка выходного представления глифов: величина отступа, порядок байтов, формат комментариев

## Use-case диаграмма
![Image](docs/img/use-case-diagram.svg)

## Черновые эскизы экранов приложения
![Image](docs/img/registration_view.svg)
![Image](docs/img/auth_view.svg)
![Image](docs/img/converter_view.svg)
![Image](docs/img/settings_view.svg)

## ER-диаграмма сущностей системы
![Image](docs/img/er-diagram.svg) 

## Портрет пользователя
* программист 
* разрабатывает встроенное приложение
* разрабатываемое приложение зависит от PEG библиотеки
* у пользователя мало времени разрабатывать свое решение
* пользователь не желает скачивать и устанавливать приложение на компьютер
 

## Сценарии использования
1) Регистрация
2) Авторизация
3) Первая конвертация шрифта в PEG
	- настройка конфигурации или выбор существущей
	- выбор доступного шрифта
	- ввод символов (наблона)
	- копирование результата конвертации
4) Выбор ранее произведенной конвертации
	- выбор существующей конвертации
	- копирование результата конвертации
	