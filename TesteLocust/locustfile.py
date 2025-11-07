from locust import HttpUser, task, between, SequentialTaskSet
import random
import json


class BlogUser(HttpUser):
    """
    Usuário básico que apenas visualiza conteúdo (não autenticado)
    """
    wait_time = between(1, 3)
    host = "http://localhost:5000"  # Ajuste conforme necessário

    def on_start(self):
        """Executado quando o usuário inicia"""
        pass

    @task(3)
    def get_home(self):
        """Testa o endpoint Home"""
        with self.client.get("/Home", catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(5)
    def get_posts(self):
        """Lista todos os posts"""
        with self.client.get("/Post/v1/posts", catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(3)
    def get_post_detail(self):
        """Busca detalhes de um post específico"""
        # Usando IDs comuns (1-10) para teste
        post_id = random.randint(1, 10)
        with self.client.get(f"/Post/v1/posts/{post_id}", catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            elif response.status_code == 404:
                response.success()  # 404 é esperado para alguns IDs
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(2)
    def get_posts_by_category(self):
        """Busca posts por categoria"""
        categories = ["backend", "frontend", "mobile", "devops", "cloud"]
        category = random.choice(categories)
        with self.client.get(f"/Post/v1/posts/category/{category}", catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(2)
    def get_categories(self):
        """Lista todas as categorias"""
        with self.client.get("/v1/categories", catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(1)
    def get_category_detail(self):
        """Busca detalhes de uma categoria específica"""
        category_id = random.randint(1, 10)
        with self.client.get(f"/v1/categories/{category_id}", catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            elif response.status_code == 404:
                response.success()  # 404 é esperado para alguns IDs
            else:
                response.failure(f"Status code: {response.status_code}")


class AuthenticatedUser(HttpUser):
    """
    Usuário autenticado que pode criar e gerenciar categorias
    """
    wait_time = between(2, 5)
    host = "http://localhost:5000"
    token = None

    def on_start(self):
        """Faz login ao iniciar"""
        self.login()

    def login(self):
        """Realiza login e armazena o token"""
        # Você precisa ter um usuário válido no banco de dados
        login_data = {
            "email": "teste@teste.com",  # Ajuste com um email válido
            "password": "senha123"  # Ajuste com uma senha válida
        }
        
        with self.client.post("/accounts/login", 
                            json=login_data,
                            catch_response=True) as response:
            if response.status_code == 200:
                try:
                    result = response.json()
                    if result.get("data"):
                        self.token = result["data"]
                        response.success()
                    else:
                        response.failure("Token não encontrado na resposta")
                except:
                    response.failure("Resposta inválida")
            else:
                response.failure(f"Falha no login: {response.status_code}")

    @task(3)
    def get_posts(self):
        """Lista todos os posts"""
        headers = {"Authorization": f"Bearer {self.token}"} if self.token else {}
        with self.client.get("/Post/v1/posts", 
                           headers=headers,
                           catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(2)
    def get_categories(self):
        """Lista todas as categorias"""
        headers = {"Authorization": f"Bearer {self.token}"} if self.token else {}
        with self.client.get("/v1/categories",
                           headers=headers,
                           catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(1)
    def create_category(self):
        """Cria uma nova categoria"""
        if not self.token:
            return
            
        category_data = {
            "name": f"Categoria Teste {random.randint(1000, 9999)}",
            "slug": f"categoria-teste-{random.randint(1000, 9999)}"
        }
        
        headers = {"Authorization": f"Bearer {self.token}"}
        with self.client.post("/v1/categories",
                            json=category_data,
                            headers=headers,
                            catch_response=True) as response:
            if response.status_code == 201:
                response.success()
            elif response.status_code == 400:
                response.success()  # Pode ser categoria duplicada
            else:
                response.failure(f"Status code: {response.status_code}")

    @task(1)
    def update_category(self):
        """Atualiza uma categoria existente"""
        if not self.token:
            return
            
        category_id = random.randint(1, 5)  # IDs que provavelmente existem
        category_data = {
            "name": f"Categoria Atualizada {random.randint(1000, 9999)}",
            "slug": f"categoria-atualizada-{random.randint(1000, 9999)}"
        }
        
        headers = {"Authorization": f"Bearer {self.token}"}
        with self.client.put(f"/v1/categories/{category_id}",
                           json=category_data,
                           headers=headers,
                           catch_response=True) as response:
            if response.status_code == 200:
                response.success()
            elif response.status_code == 404:
                response.success()  # Categoria não encontrada é esperado
            else:
                response.failure(f"Status code: {response.status_code}")


class RegistrationUser(HttpUser):
    """
    Usuário que testa o fluxo de registro
    """
    wait_time = between(3, 6)
    host = "http://localhost:5000"

    @task(1)
    def register_user(self):
        """Registra um novo usuário"""
        # Gera email único para evitar conflitos
        random_id = random.randint(10000, 99999)
        register_data = {
            "name": f"Usuário Teste {random_id}",
            "email": f"teste{random_id}@exemplo.com"
        }
        
        with self.client.post("/v1/accounts",
                            json=register_data,
                            catch_response=True) as response:
            if response.status_code == 200:
                try:
                    result = response.json()
                    if result.get("data"):
                        response.success()
                    else:
                        response.failure("Dados não encontrados na resposta")
                except:
                    response.failure("Resposta inválida")
            elif response.status_code == 400:
                response.success()  # Email já cadastrado é esperado
            else:
                response.failure(f"Status code: {response.status_code}")


class SequentialBlogUser(HttpUser):
    """
    Usuário que segue um fluxo sequencial de navegação
    """
    wait_time = between(1, 2)
    host = "http://localhost:5000"

    @task
    class UserFlow(SequentialTaskSet):
        """Fluxo sequencial de navegação"""
        
        def on_start(self):
            """Inicia o fluxo"""
            pass

        @task
        def step1_get_home(self):
            """Passo 1: Acessa a home"""
            self.client.get("/Home")

        @task
        def step2_get_categories(self):
            """Passo 2: Lista categorias"""
            self.client.get("/v1/categories")

        @task
        def step3_get_posts(self):
            """Passo 3: Lista posts"""
            self.client.get("/Post/v1/posts")

        @task
        def step4_get_post_detail(self):
            """Passo 4: Visualiza detalhes de um post"""
            post_id = random.randint(1, 10)
            self.client.get(f"/Post/v1/posts/{post_id}")

        @task
        def step5_get_posts_by_category(self):
            """Passo 5: Filtra posts por categoria"""
            categories = ["backend", "frontend", "mobile"]
            category = random.choice(categories)
            self.client.get(f"/Post/v1/posts/category/{category}")
