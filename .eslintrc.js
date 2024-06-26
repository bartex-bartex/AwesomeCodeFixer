module.exports = {
    "env": {
        "browser": true,
        "es2021": true
    },
    "extends": "eslint:recommended",
    "parser": "@babel/eslint-parser",
    "parserOptions": {
        requireConfigFile: false, // <== ADD THIS
        sourceType: 'script', // Allows for the use of imports
    },
    rules: {
      "no-unused-vars": "off",
    },
    "overrides": [{
      "files": ["*.md"],
      "parser": "eslint-plugin-markdownlint/parser",
      "extends": ["plugin:markdownlint/recommended"],
      "rules": {
        "markdownlint/md001": "off",
        "markdownlint/md003": "warn",
        "markdownlint/md025": ["error", {
          "level": 2
        }]
      }
    }]
  }