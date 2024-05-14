## Instalation
1. Clone project
2. Install Node.js
3. Install Ubuntu on WSL

*Note:* Pip install is a global configuration, however you can go with .venv setup.

### Install Linters
1. Install linting tools in root directory
   
```
npm install --save-dev eslint@8.57.0
npm install eslint-plugin-markdownlint --save-dev
npm install eslint @babel/core @babel/eslint-parser --save-dev
```

```
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
```

```
pip install black
pip install clang-format
```
