#!/bin/bash

# ===========================================
# Generate C# .NET Core Client
# ===========================================
echo "Generating C# PetStore client..."

rm -rf Apis/Clients

npx @openapitools/openapi-generator-cli generate -o Apis -g csharp-netcore -i \
    https://petstore3.swagger.io/api/v3/openapi.json \
    --additional-properties=buildTarget=library \
    --additional-properties=aspnetCoreVersion=6.0 \
    --additional-properties=operationIsAsync=true \
    --additional-properties=operationResultTask=true \
    --additional-properties=packageName=PetStoreClient \
    --additional-properties=apiName=PetStoreClient \
    --additional-properties=apiName=PetStore \
    --additional-properties=sourceFolder=Clients \
    --global-property apiTests=false,modelTests=false

echo "C# client generated successfully!"

# ===========================================
# Generate TypeScript Fetch Client
# ===========================================
echo "Generating TypeScript Fetch client..."

rm -rf ts-client

npx @openapitools/openapi-generator-cli generate -o ts-client -g typescript-fetch -i \
    https://petstore3.swagger.io/api/v3/openapi.json \
    --additional-properties=npmName=petstore-ts-client \
    --additional-properties=npmVersion=1.0.0 \
    --additional-properties=supportsES6=true \
    --additional-properties=typescriptThreePlus=true \
    --additional-properties=withInterfaces=true \
    --global-property apiTests=false,modelTests=false

# Create package.json if not exists
if [ ! -f ts-client/package.json ]; then
    cat > ts-client/package.json << 'EOF'
{
  "name": "petstore-ts-client",
  "version": "1.0.0",
  "description": "TypeScript Fetch client for PetStore API",
  "main": "dist/index.js",
  "types": "dist/index.d.ts",
  "scripts": {
    "build": "tsc",
    "prepublishOnly": "npm run build"
  },
  "devDependencies": {
    "typescript": "^5.0.0"
  },
  "files": [
    "dist/**/*"
  ]
}
EOF
fi

# Create tsconfig.json if not exists
if [ ! -f ts-client/tsconfig.json ]; then
    cat > ts-client/tsconfig.json << 'EOF'
{
  "compilerOptions": {
    "target": "ES6",
    "module": "commonjs",
    "lib": ["ES6", "DOM"],
    "declaration": true,
    "strict": true,
    "noImplicitAny": true,
    "strictNullChecks": true,
    "noImplicitThis": true,
    "alwaysStrict": true,
    "noUnusedLocals": false,
    "noUnusedParameters": false,
    "noImplicitReturns": true,
    "noFallthroughCasesInSwitch": false,
    "inlineSourceMap": true,
    "inlineSources": true,
    "experimentalDecorators": true,
    "strictPropertyInitialization": false,
    "outDir": "./dist",
    "rootDir": "./src"
  },
  "include": ["src/**/*"],
  "exclude": ["node_modules", "dist"]
}
EOF
fi

echo "TypeScript Fetch client generated successfully!"
echo ""
echo "To use the TypeScript client:"
echo "  cd ts-client"
echo "  npm install"
echo "  npm run build"