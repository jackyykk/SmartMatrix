import path from "node:path";
import { fileURLToPath } from "node:url";
import js from "@eslint/js";
import { FlatCompat } from "@eslint/eslintrc";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
    baseDirectory: __dirname,
    recommendedConfig: js.configs.recommended,
    allConfig: js.configs.all
});

const eslintConfig = [
    ...compat.extends("next/core-web-vitals", "next/typescript"),
    {
        files: ["**/*.ts", "**/*.tsx"],
        parser: "@typescript-eslint/parser",
        parserOptions: {
            ecmaVersion: 2020,
            sourceType: "module",
            project: "./tsconfig.json", // Ensure it points to your tsconfig.json
            tsconfigRootDir: __dirname,  // Set the root directory for the tsconfig
        },
        plugins: ["@typescript-eslint"],
        rules: {
            // Add your custom rules here
        }
    }
];

export default eslintConfig;