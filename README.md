## Instalation
1. Clone project
2. Install latest Node.js
3. Install Linux distro (developed with Ubuntu) on WSL
4. Create venv for python in the root of repository

*Note:* Add path to your current node version bin folder to .bashrc, e.g. 
```
export PATH="$PATH:/home/bartex/.nvm/versions/node/v20.13.1/bin/"
```
## Installing Linters and Formatters
*Note:* Below operations perform in repository root directory with activated venv.

*Important:* Remember to alter paths to tools in Formatter.cs and Linter.cs. 

### Install Linters
   
```
npm install --save-dev eslint@8.57.0
npm install eslint-plugin-markdownlint --save-dev
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
## Using tools reference 

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
