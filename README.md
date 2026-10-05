# BM@N Desktop

BM@N Desktop — проект на Unity для просмотра 3D-модели экспериментальной установки BM@N.

## Требования

- [Git](https://git-scm.com/)
- [Unity Hub](https://unity.com/download)
- Unity `6000.2.11f1`

## Установка

Склонируйте репозиторий:

```bash
git clone https://github.com/kostan1351/BMN-Desktop.git
cd BMN-Desktop
```

Добавьте папку проекта в Unity Hub и откройте её с помощью Unity `6000.2.11f1`. При первом запуске Unity автоматически импортирует файлы и установит необходимые пакеты.

## Запуск

Откройте сцену `Assets/Scenes/0-MainScence.unity` и нажмите кнопку **Play** в верхней части редактора Unity.

## Окружение

Базовое окружение для просмотра установки — технический зал `InstallationRoom` в сцене `2-SampleScene`. Он сохранён в `Assets/Prefabs/InstallationRoom.prefab`: стены, пол, потолочные панели и свет можно менять отдельно от модели установки.

В зале есть фактуры пола и стен, разметка, табличка BM@N и декоративный вход. Текстуры находятся в `Assets/Textures`, отражения сохранены заранее.

## Обновление проекта

Чтобы скачать последние изменения:

```bash
git pull origin main
```

## Готовая сборка

Готовые версии проекта находятся на [странице Releases](https://github.com/kostan1351/BMN-Desktop/releases).
