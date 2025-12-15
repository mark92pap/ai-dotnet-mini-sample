rm -rf ./Apis/Server/Server

npx @openapitools/openapi-generator-cli generate \
						-o Apis/Server \
						-g aspnetcore \
						-i openapispec.json \
						--template-dir Template \
						--additional-properties=buildTarget=library \
						--additional-properties=operationResultTask=true \
						--additional-properties=aspnetCoreVersion=6.0 \
						--additional-properties=operationIsAsync=true \
						--additional-properties=packageName=GeneratedApi \
						--additional-properties=sourceFolder=Server \
						--additional-properties=classModifier=abstract \
						--additional-properties=operationModifier=abstract \
						--additional-properties=swashbuckleVersion=6.4.0 \
						--additional-properties=enumNameSuffix= \
						--additional-properties=enumValueSuffix= \
						--additional-properties=useSwashbuckle=true