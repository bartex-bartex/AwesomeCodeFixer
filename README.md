# AwesomeCodeFixer

Celem projektu jest stworzenie narzędzia do formatowania i lintowania pliku łączącego składnie:
- markdown'a
- inline/block latex'a
- języków programowania (aktualnie wspierane - C, C++, Python, Sql)

Narzędzie posiada 2 endpointy:
- format - przyjmuje treść artykułu, a na wyjście przesyła sformatowany tekst
- lint - przyjmuje treść artykułu, a na wyjście przesyła listę błędów/warning'ów

## Wykorzystane narzędzia
Język implementacji: C#.

- markdownlint: https://github.com/DavidAnson/markdownlint
- chktex: https://www.nongnu.org/chktex/
- eslint: https://eslint.org/
- prettier: https://prettier.io/
- clang-format: https://clang.llvm.org/docs/ClangFormat.html
- clang-tidy: https://clang.llvm.org/extra/clang-tidy/
- black: https://github.com/psf/black
- flake8: https://github.com/PyCQA/flake8
- sqlfluff: https://github.com/sqlfluff/sqlfluff

## Installation
1. Clone project
2. Install latest Node.js
3. Install Linux distro (developed with Ubuntu) on WSL
4. Create venv for python in the root of repository and activate it

*Note:* Add path to your current node version bin folder to .bashrc, e.g. 
```
export PATH="$PATH:/home/bartex/.nvm/versions/node/v20.13.1/bin/"
```

*Note:* Below operations perform in repository root directory with activated venv.

*Important:* Remember to alter paths to tools in Formatter.cs and Linter.cs. 

### Install Linters
   
```
npm install --save-dev eslint@8.57.0
npm install markdownlint-cli --save-dev
npm install eslint @babel/core @babel/eslint-parser --save-dev
pip install cpplint
pip install sqlfluff
pip install flake8
sudo apt install chktex
```

### Install Formatters
```
npm install --save-dev --save-exact prettier
npm install --save-dev prettier-plugin-latex
npm install --save-dev prettier-plugin-sql
pip install black
pip install clang-format
```
## Tools usage reference 

### Running linters:
```
npx eslint --stdin --stdin-filename=foo.md
chktex
clang-format --dry-run --Werror --assume-filename=.c
clang-format --dry-run --Werror --assume-filename=.cpp
sqlfluff lint - --dialect ansi
flake8 -
```

### Running formatters:
```
prettier --stdin-filepath foo.xxx
black -
clang-format --assume-filename=<string> 
```

## Contribution
- Bartosz Warchoł
- Radosław Myśliwiec
- Gabriela Żmuda
