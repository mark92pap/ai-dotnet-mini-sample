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