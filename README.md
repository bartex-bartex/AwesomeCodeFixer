## Instalation
1. Clone project
2. Install Node.js
3. Install Ubuntu on WSL

*Note:* Pip install is a global configuration, however you can go with .venv setup.
*Note:* Add this line to .bashrc 
```
export PATH="$PATH:/home/bartex/.nvm/versions/node/v20.13.1/bin/"
```

### Install Linters
1. Install linting tools in root directory
   
```
npm install --save-dev eslint@8.57.0
npm install eslint-plugin-markdownlint --save-dev
npm install eslint @babel/core @babel/eslint-parser --save-dev
pip install cpplint
pip install sqlfluff
pip install flake8
```

2. Install chktex on Ubuntu
```
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

npm install eslint@8.57.0
npm install eslint-plugin-markdownlint
npm install eslint @babel/core @babel/eslint-parser
npm install --save-exact prettier
npm install prettier-plugin-latex
npm install prettier-plugin-sql


### Running linters:
npx eslint --stdin --stdin-filename=foo.md
chktex
clang-format --dry-run --Werror --assume-filename=.c
clang-format --dry-run --Werror --assume-filename=.cpp
sqlfluff lint - --dialect ansi
flake8 -


### Running formatters:
prettier --stdin-filepath foo.xxx
black -
clang-format --assume-filename=<string> 
